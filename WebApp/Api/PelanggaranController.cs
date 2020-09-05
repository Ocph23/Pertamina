using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using WebApp.Middlewares;

namespace WebApp.Api
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PelanggaranController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public PelanggaranController(IConfiguration config, ApplicationDbContext dbcontext)
        {
            _config = config;
            _context = dbcontext;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            var result = _context.Pelanggaran
            .Include(x => x.DataKaryawan)
            .Include(x => x.DataPerusahaan)
            .Include(x => x.Jenispelanggaran).ThenInclude(x => x.Level)
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
            var datas = _context.Pelanggaran.Where(x => x.KaryawanId == id)
                 .Include(z => z.Jenispelanggaran)
                 .Include(x => x.Files);

            var result = from a in datas
                         join c in _context.Level on a.Jenispelanggaran.LevelId equals c.Id
                         select new Pelanggaran
                         {
                             Files = a.Files,
                             JenisPelanggaranId = a.JenisPelanggaranId,
                             KaryawanId = a.KaryawanId,
                             Id = a.Id,
                             Jenispelanggaran = a.Jenispelanggaran,
                             NilaiKaryawan = a.NilaiKaryawan,
                             NilaiPerusahaan = a.NilaiPerusahaan,
                             Tanggal = a.Tanggal,
                             Level = c
                         };
            return Ok(result.ToList());

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
        public IActionResult Post([FromBody] Pelanggaran value)
        {

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
                    _context.Pelanggaran.Add(value);
                    var saved = _context.SaveChanges();
                    if (value.Id <= 0)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");


                    transaction.Commit();

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
                var pelanggaran = _context.Pelanggaran.Where(x => x.JenisPelanggaranId == value.Id).FirstOrDefault();
                pelanggaran.JenisPelanggaranId = value.JenisPelanggaranId;
                pelanggaran.KaryawanId = value.Id;
                pelanggaran.Jenispelanggaran = value.Jenispelanggaran;
                pelanggaran.NilaiKaryawan = value.NilaiKaryawan;
                pelanggaran.NilaiPerusahaan = value.NilaiPerusahaan;
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