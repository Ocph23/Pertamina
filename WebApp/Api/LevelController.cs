using System;
using System.Linq;
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
    [ApiAuthorize]
    public class LevelController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public LevelController(IConfiguration config, ApplicationDbContext dbcontext)
        {
            _config = config;
            _context = dbcontext;
        }

        // GET: api/Employees
        [HttpGet]
        [ApiAuthorize]
        public IActionResult Get()
        {
            try
            {
                var results = _context.Level.Include(x => x.Datas);
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
                var result = _context.Level.Where(x => x.Id == id).Include(x => x.Datas).FirstOrDefault();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Level value)
        {
            try
            {
                _context.Level.Add(value);
                _context.SaveChanges();

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
        public IActionResult Put(int id, [FromBody] Level value)
        {
            try
            {
                var result = _context.Level.Where(x => x.Id == value.Id).FirstOrDefault();
                result.Nama = value.Nama;
                var saved = _context.SaveChanges();
                if (saved <= 0)
                    throw new SystemException("Data Tidak Berhasil Disimpan !");

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
                var result = _context.Level.Where(x => x.Id == id).FirstOrDefault();
                if (result == null)
                    throw new SystemException("Data Tidak Ditemukan");
                _context.Level.Remove(result);

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