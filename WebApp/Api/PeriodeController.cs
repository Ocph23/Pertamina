using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class PeriodeController : ControllerBase
    {
        private IFcmService _fmc;
        private IConfiguration _config;
        private ApplicationDbContext _context;
        private IPeriodeService _periode;
        TimeZoneInfo nzTimeZone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Tokyo Standard Time") ?
        TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time") : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

        public PeriodeController(IConfiguration config, ApplicationDbContext context, IPeriodeService periode, IFcmService fcm)
        {
            _fmc = fcm;
            _config = config;
            _context = context;
            _periode = periode;
        }

        // GET: api/Employees
        [ApiAuthorize]
        [HttpGet]

        public IActionResult Get()
        {
            try
            {
                var data = _context.Periode.ToList();
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Employees/5
        [ApiAuthorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_context.Periode.Where(x => x.Id == id).FirstOrDefault());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("active")]
     
        public async Task<IActionResult> Active()
        {
            try
            {
                var active = await _periode.ActivePeriode();

                return Ok(active);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [ApiAuthorize(Roles = "admin, administrator")]
        [HttpPost("pemenang")]
        public async Task<IActionResult> Pemenang([FromBody] Pemenang pemenang)
        {
            try
            {
                _context.Pemenang.Add(pemenang);
                var result = _context.SaveChanges();
                if (result <= 0)
                {
                    throw new SystemException("Pemenang Tidak Berhasil Disimpan");
                }
                var karyawan = _context.Karyawan.Where(x => x.Id == pemenang.KaryawanId).FirstOrDefault();
                if (karyawan != null)
                {
                    var message = new NotificationModel("System", "Undian",
                          $"Pemenang Undian Peride Ini '{karyawan.NamaKaryawan}'", NotificationType.Public);

                    var message1 = new NotificationModel("System", "Undian",
                          $"Anda Adalah Pemeang Undian Periode Ini", NotificationType.Private);
                    message1.UserId = karyawan.UserId;

                    await _fmc.SendMessage(message,"all");
                    await _fmc.SendMessagePrivate(message1, karyawan.DeviceId);
                }
                return Ok(true);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        // POST: api/Employees
        [HttpPost]
        [ApiAuthorize]
        public IActionResult Post([FromBody] Periode value)
        {
            try
            {
                value.Mulai = TimeZoneInfo.ConvertTimeFromUtc(value.Mulai, nzTimeZone);
                value.Selesai = TimeZoneInfo.ConvertTimeFromUtc(value.Selesai, nzTimeZone);
                value.Undian = TimeZoneInfo.ConvertTimeFromUtc(value.Undian, nzTimeZone);
                value.Status = true;
                var active = _context.Periode.Where(x => x.Status == true).FirstOrDefault();
                if (active != null)
                {
                    if (value.Mulai < active.Selesai)
                        throw new SystemException("Tanggal Mulai Harus Lebih Besar dari Tanggal Akhir Periode Lalu");
                    active.Status = false;
                }
                _context.Periode.Add(value);

                var saved = _context.SaveChanges();
                if (saved <= 0)
                    throw new SystemException("Data Periode  Tidak Berhasil Disimpan !");
                return Ok(value);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        [ApiAuthorize]
        public IActionResult Put(int id, [FromBody] Periode value)
        {
            try
            {
                value.Mulai = TimeZoneInfo.ConvertTimeFromUtc(value.Mulai, nzTimeZone);
                value.Selesai = TimeZoneInfo.ConvertTimeFromUtc(value.Selesai, nzTimeZone);
                value.Undian = TimeZoneInfo.ConvertTimeFromUtc(value.Undian, nzTimeZone);
                var periode = _context.Periode.Where(x => x.Id == value.Id).FirstOrDefault();
                periode.Id = value.Id;
                periode.Mulai = value.Mulai;
                periode.Selesai = value.Selesai;
                periode.Undian = value.Undian;

                var updated = _context.SaveChanges();
                if (updated <= 0)
                    throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");

                return Ok(value);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ApiAuthorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _context.Periode.Where(x => x.Id == id).FirstOrDefault();
                _context.Periode.Remove(deleted);
                var result = _context.SaveChanges();
                if (result <= 0)
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