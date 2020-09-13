using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Models;
using WebApp.Middlewares;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AbsenController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;
        private IAbsenModel _absen;

        public AbsenController(IConfiguration config, ApplicationDbContext dbcontext, IAbsenModel absenService)
        {
            _config = config;
            _context = dbcontext;
            _absen = absenService;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var activePeriode = _context.Periode.Where(x => x.Status).FirstOrDefault();

                var results = _context.Absen.Where(x => x.Masuk >= activePeriode.Mulai && x.Masuk <= activePeriode.Selesai)
                            .Include(x => x.Karyawan);

                return Ok(results.ToList());
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
                var result = _context.Level.Where(x => x.Id == id).FirstOrDefault();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Absen value)
        {
            try
            {
                var result = await _absen.absen(value.KaryawanId, value.AbsenType);
                if (result == null)
                    throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Absen value)
        {
            try
            {
                var result = _context.Level.Where(x => x.Id == value.Id).FirstOrDefault();
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
                var result = _context.Absen.Where(x => x.Id == id).FirstOrDefault();
                _context.Absen.Remove(result);

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