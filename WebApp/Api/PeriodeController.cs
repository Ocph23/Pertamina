using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return Ok(_context.Periode.Where(x => x.Id == id).FirstOrDefault());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("active")]
        public IActionResult Active()
        {
            try
            {
                return Ok(_context.Periode.Where(x => x.Status == true).FirstOrDefault());
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