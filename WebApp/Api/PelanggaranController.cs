using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;
using Newtonsoft.Json;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class PelanggaranController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public PelanggaranController(IConfiguration config, ApplicationDbContext dbcontext)
        {
            _config = config;
            _context = dbcontext;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Pelanggaran.ToList());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Pelanggaran.FirstOrDefault());
        }


        [HttpGet("karyawan/{id}")]
        public IActionResult GetByKaryawanId(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                var datas = _context.Pelanggaran.Where(x => x.idkaryawan == id)
                .Include(x => x.Files)
                .Include(z => z.Jenispelanggaran);

                var result = from a in datas
                             join c in _context.Level on a.Jenispelanggaran.idlevel equals c.idlevel
                             select new Pelanggaran
                             {
                                 Files = a.Files,
                                 idjenispelanggaran = a.idjenispelanggaran,
                                 idkaryawan = a.idkaryawan,
                                 idpelanggaran = a.idpelanggaran,
                                 Jenispelanggaran = a.Jenispelanggaran,
                                 karyawan = a.karyawan,
                                 perusahaan = a.perusahaan,
                                 tanggal = a.tanggal,
                                 Level = c
                             };
                return Ok(result.ToList());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Pelanggaran value)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in value.Files)
                    {
                        var path = Helpers.GetPath(item.FileType);
                        item.FileName = Helpers.CreateFileName(item.FileType);
                        item.Thumb = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(path + item.FileName, item.Data);
                        System.IO.File.WriteAllBytes(Helpers.ThumbPath + item.Thumb, Helpers.CreateThumb(item.Data));
                        item.Data = null;
                    }
                    _context.Pelanggaran.Add(value);
                    var saved = _context.SaveChanges();
                    if (value.idpelanggaran <= 0)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");


                    transaction.Commit();

                    return Ok(value);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }



        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pelanggaran value)
        {
            try
            {
                var pelanggaran = _context.Pelanggaran.Where(x => x.idjenispelanggaran == value.idpelanggaran).FirstOrDefault();
                pelanggaran.idjenispelanggaran = value.idjenispelanggaran;
                pelanggaran.idkaryawan = value.idpelanggaran;
                pelanggaran.Jenispelanggaran = value.Jenispelanggaran;
                pelanggaran.karyawan = value.karyawan;
                pelanggaran.perusahaan = value.perusahaan;
                pelanggaran.tanggal = value.tanggal;
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
                var item = _context.Pelanggaran.Where(x => x.idpelanggaran == id).FirstOrDefault();
                _context.Pelanggaran.Remove(item);
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