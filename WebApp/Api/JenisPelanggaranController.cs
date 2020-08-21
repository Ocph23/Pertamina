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
    public class JenisPelanggaranController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public JenisPelanggaranController(IConfiguration config, ApplicationDbContext dbcontext)
        {
            _config = config;
            _context = dbcontext;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _context.JenisPelanggaran.ToList();
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
            var data = _context.JenisPelanggaran.FirstOrDefault();
            return Ok(data);
        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Jenispelanggaran value)
        {
            try
            {
                _context.JenisPelanggaran.Add(value);
                _context.SaveChanges();
                return Ok(value);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Jenispelanggaran value)
        {
            try
            {
                var data = _context.JenisPelanggaran.Where(x => x.idjenispelanggaran == value.idjenispelanggaran).FirstOrDefault();
                if (data != null)
                {
                    data.jenispelanggaran = value.jenispelanggaran;
                    data.penambahanpoint = value.penambahanpoint;
                    data.pengurangankaryawan = value.pengurangankaryawan;
                    data.penguranganperusahaan = value.penguranganperusahaan;
                }
                _context.SaveChanges();
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
                var data = _context.JenisPelanggaran.Where(x => x.idjenispelanggaran == id).FirstOrDefault();
                _context.JenisPelanggaran.Remove(data);
                _context.SaveChanges();
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}