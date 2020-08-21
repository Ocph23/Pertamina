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
    public class PerusahaanController : ControllerBase
    {
        private IConfiguration _config;

        public ApplicationDbContext _context { get; private set; }

        public PerusahaanController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Perusahaan.ToList());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Perusahaan.Where(x => x.idperusahaan == id).FirstOrDefault());

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Perusahaan value)
        {
            try
            {
                if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                {
                    value.logo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.logo, Helpers.CreateThumb(value.DataPhoto));
                }
                _context.Perusahaan.Add(value);
                var saved = _context.SaveChanges();
                if (value.idperusahaan <= 0)
                    throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");
                return Ok(value);
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
                if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                {
                    value.logo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.LogoPath + value.logo, Helpers.CreateThumb(value.DataPhoto));
                }
                var x = _context.Perusahaan.Where(x => x.idperusahaan == value.idperusahaan).FirstOrDefault();
                x.idperusahaan = value.idperusahaan;
                x.alamat = value.alamat;
                x.direktur = value.direktur;
                x.emaildirektur = value.emaildirektur;
                x.kontakdirektur = value.kontakdirektur;
                x.logo = value.logo;
                x.namaperusahaan = value.namaperusahaan;

                var saved = _context.SaveChanges();

                if (saved <= 0)
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
                var item = _context.Perusahaan.Where(x => x.idperusahaan == id).FirstOrDefault();

                _context.Perusahaan.Remove(item);
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