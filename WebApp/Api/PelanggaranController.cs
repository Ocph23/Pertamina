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

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PelanggaranController : ControllerBase
    {
        private IConfiguration _config;

        public PelanggaranController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Pelanggaran.Select());
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                return Ok(db.Level.Where(x => x.idlevel == id).FirstOrDefault());
            }

        }


        [HttpGet("karyawan/{id}")]
        public IActionResult GetByKaryawanId(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = @"SELECT
                    `buktipelanggaran`.`Id`,
                    `buktipelanggaran`.`filetype`,
                    `buktipelanggaran`.`filename`,
                    `buktipelanggaran`.`thumb`,
                    `buktipelanggaran`.`idpelanggaran`
                    FROM
                    `pelanggaran`
                    Right JOIN `buktipelanggaran` ON `pelanggaran`.`idpelanggaran` =
                    `buktipelanggaran`.`idpelanggaran`
                    where idkaryawan = " + id;

                var result = db.Query<DataFile>(sql);

                var datas = from a in db.Pelanggaran.Where(x => x.idkaryawan == id)
                            join d in db.JenisPelanggaran.Select() on a.idjenispelanggaran equals d.idjenispelanggaran
                            join f in db.Level.Select() on d.idlevel equals f.idlevel
                            join c in result on a.idpelanggaran equals c.IdPelanggaran into gbukti
                            select new Pelanggaran
                            {
                                Level = f,
                                Jenispelanggaran = d,
                                idjenispelanggaran = a.idjenispelanggaran,
                                idkaryawan = a.idkaryawan,
                                idpelanggaran = a.idpelanggaran,
                                karyawan = a.karyawan,
                                perusahaan = a.perusahaan,
                                tanggal = a.tanggal,
                                Files = gbukti.ToList()
                            };


                return Ok(datas.ToList());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post([FromBody] Pelanggaran value)
        {

            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                var trans = db.BeginTransaction();
                try
                {
                    value.idpelanggaran = db.Pelanggaran.InsertAndGetLastID(value);
                    if (value.idpelanggaran <= 0)
                        throw new SystemException("Data Perusahaan  Tidak Berhasil Disimpan !");

                    foreach (var item in value.Files)
                    {
                        var path = Helpers.GetPath(item.FileType);
                        item.FileName = Helpers.CreateFileName(item.FileType);
                        item.Thumb = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(path + item.FileName, item.Data);
                        System.IO.File.WriteAllBytes(Helpers.ThumbPath + item.Thumb, Helpers.CreateThumb(item.Data));
                        item.Data = null;
                        item.IdPelanggaran = value.idpelanggaran;
                        item.Id = db.BuktiPelanggaran.InsertAndGetLastID(item);
                    }
                    trans.Commit();
                    return Ok(value);
                }
                catch (System.Exception ex)
                {
                    trans.Rollback();
                    return BadRequest(ex.Message);
                }

            }

        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Level value)
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var updated = db.Level.Update(x => new
                    {
                        x.level
                    }, value, x => x.idlevel == value.idlevel);
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
                    var deleted = db.Level.Delete(x => x.idlevel == id);
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