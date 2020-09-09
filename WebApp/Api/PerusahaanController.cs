using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Middlewares;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var result = _context.Perusahaan.Where(x => x.Id == id)
            .Include(x => x.PerusahaansKaryawan)
            .ThenInclude(x => x.Karyawan)
              .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x).ThenInclude(x => x.Terlapor)
                     .Include(x => x.Pelanggarans)
              .ThenInclude(x => x).ThenInclude(x => x.Pelapor)
              .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x.Files);

            return Ok(result.FirstOrDefault());

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Perusahaan value)
        {
            try
            {
                if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                {
                    value.Logo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.LogoPath + value.Logo, Helpers.CreateThumb(value.DataPhoto));
                }
                _context.Perusahaan.Add(value);
                var saved = _context.SaveChanges();
                if (value.Id <= 0)
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
                    value.Logo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.LogoPath + value.Logo, Helpers.CreateThumb(value.DataPhoto));
                }
                var x = _context.Perusahaan.Where(z => z.Id == value.Id).FirstOrDefault();
                x.Id = value.Id;
                x.Alamat = value.Alamat;
                x.Direktur = value.Direktur;
                x.Email = value.Email;
                x.Kontak = value.Kontak;
                x.Logo = value.Logo;
                x.Nama = value.Nama;

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
                var item = _context.Perusahaan.Where(x => x.Id == id).FirstOrDefault();

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