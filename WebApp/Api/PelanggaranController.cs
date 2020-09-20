using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using WebApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApp.Api
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class PelanggaranController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private IFcmService _fcm;

        public PelanggaranController(IConfiguration config, ApplicationDbContext dbcontext,
            IFcmService fcm,
            UserManager<IdentityUser> userManager)
        {
            _config = config;
            _context = dbcontext;
            _userManager = userManager;
            _fcm = fcm;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            var result = _context.Pelanggaran
            .Include(x => x.Terlapor)
            .Include(x => x.Pelapor)
            .Include(x => x.ItemPelanggarans).ThenInclude(x => x.DetailLevel).ThenInclude(x => x.Level)
            .Include(x=>x.Perusahaan)
            .Include(x => x.Files);
            return Ok(result.ToList());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Pelanggaran.FirstOrDefault());
        }


        [HttpGet("NilaiKaryawan/{id}")]
        public IActionResult GetByKaryawanId(int id)
        {
            var datas = _context.Pelanggaran.Where(x => x.Terlapor.Id == id)
                 .Include(z => z.ItemPelanggarans)
                 .Include(x => x.Files);

            // var result = from a in datas
            //              join c in _context.Level on a.Jenispelanggaran.LevelId equals c.Id
            //              select new Pelanggaran
            //              {
            //                  Files = a.Files,
            //                  JenisPelanggaranId = a.JenisPelanggaranId,
            //                  KaryawanId = a.KaryawanId,
            //                  Id = a.Id,
            //                  Jenispelanggaran = a.Jenispelanggaran,
            //                  NilaiKaryawan = a.NilaiKaryawan,
            //                  NilaiPerusahaan = a.NilaiPerusahaan,
            //                  Tanggal = a.Tanggal,
            //                  Level = c
            //              };
            return Ok(datas.ToList());

        }



        [HttpGet("status/{id}/{status}")]
        public IActionResult UpdateStatus(int id, StatusPelanggaran status)
        {
            try
            {
                var data = _context.Pelanggaran.Where(x => x.Id == id).FirstOrDefault();
                if (data != null && data.Status != status)
                {
                    data.Status = status;
                    var result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        throw new SystemException("Data Tidak Berhasil Diubah");
                    }
                }
                return Ok(status);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/Employees
        [HttpPost]

        public async Task<IActionResult> Post([FromBody] Pelanggaran value)
        {
            var user = await _userManager.GetUserAsync(User);
            var pelapor = _context.Karyawan.Where(x => x.UserId == user.Id).FirstOrDefault();
            var terlapor = _context.Karyawan.Where(x => x.Id == value.TerlaporId).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in value.Files)
                    {
                        var path = Helpers.GetPath(item.FileType);
                        item.FileName = Helpers.CreateFileName(item.FileType);
                        item.Thumb = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(path + item.FileName, item.Data);
                        System.IO.File.WriteAllBytes(Helpers.ThumbPath + item.Thumb, Helpers.CreateThumb(item.Data));
                        item.Data = null;
                    }

                    value.PelaporId = pelapor.Id;
                    value.TerlaporId = value.TerlaporId;
                    value.PerusahaanId = value.PerusahaanId;
                    value.Terlapor = null;
                    _context.Pelanggaran.Add(value);
                    var saved = _context.SaveChanges();
                    if (value.Id <= 0)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");
                    transaction.Commit();

                    var message = new NotificationModel("System", "Pelanggaran",
                            "Anda Telah Melakukan Pelanggaran !", NotificationType.Private);

                    await _fcm.SendMessagePrivate(message, terlapor.DeviceId);
                    return Ok(value);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }



        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pelanggaran value)
        {
            try
            {
                var pelanggaran = _context.Pelanggaran.Where(x => x.Id == value.Id).FirstOrDefault();
                pelanggaran.Tanggal = value.Tanggal;
                return Ok(value);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.Pelanggaran.Where(x => x.Id == id).FirstOrDefault();
                _context.Pelanggaran.Remove(item);
                var saved = _context.SaveChanges();
                if (saved <= 0)
                    throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");

                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}