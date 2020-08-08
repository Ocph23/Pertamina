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
    public class KaryawanController : ControllerBase {
        private IConfiguration _config;

        public KaryawanController (IConfiguration config) {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get () {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.Karyawan.Select ());
            }
        }

        // GET: api/Employees/5
        [HttpGet ("{id}")]
        public IActionResult GetById (int id) {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.Karyawan.Where (x => x.idkaryawan == id).FirstOrDefault ());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post ([FromBody] Karyawan value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    value.idkaryawan = db.Karyawan.InsertAndGetLastID (value);
                    if (value.idkaryawan <= 0)
                        throw new SystemException ("Data Karyawan  Tidak Berhasil Disimpan !");

                    return Ok (value);
                }
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut ("{id}")]
        public IActionResult Put (int id, [FromBody] Karyawan value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    var updated = db.Karyawan.Update (x => new { x.idperusahaan, x.jabatan, x.kontak, x.namakaryawan, x.photo }, value, x => x.idkaryawan == value.idkaryawan);
                    if (!updated)
                        throw new SystemException ("Data Karyawan  Tidak Berhasil Disimpan !");

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
                    var deleted = db.Karyawan.Delete (x => x.idkaryawan == id);
                    if (!deleted)
                        throw new SystemException ("Data Karyawan  Tidak Berhasil Disimpan !");

                    return Ok (true);
                }
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }

        }
    }
}