using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        private IConfiguration _config;
        TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

        public PeriodeController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Periode.Select().OrderByDescending(x => x.tanggalmulai));
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Periode.Where(x => x.idperiode == id).FirstOrDefault());
            }

        }


        [HttpGet("active")]
        public IActionResult Active()
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Periode.Where(x => x.status == true).FirstOrDefault());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Periode value)
        {
            value.tanggalmulai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalmulai, nzTimeZone);
            value.tanggalselesai = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalselesai, nzTimeZone);
            value.tanggalundian = TimeZoneInfo.ConvertTimeFromUtc(value.tanggalundian, nzTimeZone);
            value.status = true;





            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {

                    var active = db.Periode.Where(x => x.status).FirstOrDefault();
                    if (active != null)
                    {
                        if (value.tanggalmulai < active.tanggalselesai)
                            throw new SystemException("Tanggal Mulai Harus Lebih Besar dari Tanggal Akhir Periode Lalu");
                    }


                    var result = db.Periode.Update(x => new { x.status }, new Periode { status = false }, x => x.status == true);
                    value.idperiode = db.Periode.InsertAndGetLastID(value);
                    if (value.idperiode <= 0)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");
                    return Ok(value);
                }
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
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var updated = db.Periode.Update(x => new
                    {
                        x.idperiode,
                        x.tanggalmulai,
                        x.tanggalselesai,
                        x.tanggalundian
                    }, value, x => x.idperiode == value.idperiode);
                    if (!updated)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");

                    return Ok(value);
                }
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
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var deleted = db.Periode.Delete(x => x.idperiode == id);
                    if (!deleted)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");

                    return Ok(true);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}