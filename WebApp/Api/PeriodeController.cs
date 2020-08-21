using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;
        TimeZoneInfo nzTimeZone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Tokyo Standard Time") ?
        TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time") : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

        public PeriodeController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Employees
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
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {


            try
            {
                return Ok(_context.Periode.Where(x => x.idperiode == id).FirstOrDefault());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("active")]
        public IActionResult Active()
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    return Ok(_context.Periode.Where(x => x.status == true).FirstOrDefault());
                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Periode value)
        {

            try
            {
                value.tanggalmulai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalmulai, nzTimeZone);
                value.tanggalselesai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalselesai, nzTimeZone);
                value.tanggalundian = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalundian, nzTimeZone);
                value.status = true;
                var active = _context.Periode.Where(x => x.status == true).FirstOrDefault();
                if (active != null)
                {
                    if (value.tanggalmulai < active.tanggalselesai)
                        throw new SystemException("Tanggal Mulai Harus Lebih Besar dari Tanggal Akhir Periode Lalu");
                    active.status = false;
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
        public IActionResult Put(int id, [FromBody] Periode value)
        {
            try
            {
                value.tanggalmulai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalmulai, nzTimeZone);
                value.tanggalselesai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalselesai, nzTimeZone);
                value.tanggalundian = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalundian, nzTimeZone);
                var periode = _context.Periode.Where(x => x.idperiode == value.idperiode).FirstOrDefault();
                periode.idperiode = value.idperiode;
                periode.tanggalmulai = value.tanggalmulai;
                periode.tanggalselesai = value.tanggalselesai;
                periode.tanggalundian = value.tanggalundian;

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
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _context.Periode.Where(x => x.idperiode == id).FirstOrDefault();
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