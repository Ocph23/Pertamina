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
    public class PerusahaanController : ControllerBase
    {
        private IConfiguration _config;

        public PerusahaanController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Perusahaan.Select());
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Perusahaan.Where(x => x.idperusahaan == id).FirstOrDefault());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Perusahaan value)
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                    {
                        value.logo = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.logo, Helpers.CreateThumb(value.DataPhoto));
                    }
                    value.idperusahaan = db.Perusahaan.InsertAndGetLastID(value);

                    if (value.idperusahaan <= 0)
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
        public IActionResult Put(int id, [FromBody] Perusahaan value)
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                    {
                        value.logo = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(Helpers.LogoPath + value.logo, Helpers.CreateThumb(value.DataPhoto));
                    }
                    var updated = db.Perusahaan.Update(x => new
                    {
                        x.idperusahaan,
                        x.alamat,
                        x.direktur,
                        x.emaildirektur,
                        x.kontakdirektur,
                        x.logo,
                        x.namaperusahaan
                    }, value, x => x.idperusahaan == value.idperusahaan);
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
                    var deleted = db.Perusahaan.Delete(x => x.idperusahaan == id);
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