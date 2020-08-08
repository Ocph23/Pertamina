using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Api {
    [Route ("api/[controller]")]
    [ApiController]
    public class JenisPelanggaranController : ControllerBase {
        private IConfiguration _config;

        public JenisPelanggaranController (IConfiguration config) {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get () {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.JenisPelanggaran.Select ());
            }
        }

        // GET: api/Employees/5
        [HttpGet ("{id}")]
        public IActionResult GetById (int id) {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.JenisPelanggaran.Where (x => x.idjenispelanggaran == id).FirstOrDefault ());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post ([FromBody] Jenispelanggaran value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    value.idjenispelanggaran = db.JenisPelanggaran.InsertAndGetLastID (value);
                    if (value.idjenispelanggaran <= 0)
                        throw new SystemException ("Data Perusahaan  Tidak Berhasil Disimpan !");
                    return Ok (value);
                }
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut ("{id}")]
        public IActionResult Put (int id, [FromBody] Jenispelanggaran value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    var updated = db.JenisPelanggaran.Update (x => new {
                        x.idlevel, x.jenispelanggaran, x.penambahanpoint, x.pengurangankaryawan, x.penguranganperusahaan
                    }, value, x => x.idjenispelanggaran == value.idjenispelanggaran);
                    if (!updated)
                        throw new SystemException ("Data Perusahaan  Tidak Berhasil Disimpan !");

                    return Ok (value);
                }
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete ("{id}")]
        public IActionResult Delete (int id) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    var deleted = db.JenisPelanggaran.Delete (x => x.idjenispelanggaran == id);
                    if (!deleted)
                        throw new SystemException ("Data Perusahaan  Tidak Berhasil Disimpan !");

                    return Ok (true);
                }
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }

        }
    }
}