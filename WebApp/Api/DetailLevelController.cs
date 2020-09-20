using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Models;
using WebApp.Middlewares;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class DetailLevelController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public DetailLevelController(IConfiguration config, ApplicationDbContext dbcontext)
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
                var data = _context.DetailLevels.ToList();
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
            var data = _context.DetailLevels.FirstOrDefault();
            return Ok(data);
        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] DetailLevel value)
        {
            try
            {
                _context.DetailLevels.Add(value);
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
        public IActionResult Put(int id, [FromBody] DetailLevel value)
        {
            try
            {
                var data = _context.DetailLevels.Where(x => x.Id == value.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Nama = value.Nama;
                    data.Penambahan = value.Penambahan;
                    data.NilaiKaryawan = value.NilaiKaryawan;
                    data.NilaiPerusahaan = value.NilaiPerusahaan;
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
                var data = _context.DetailLevels.Where(x => x.Id == id).FirstOrDefault();
                _context.DetailLevels.Remove(data);
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