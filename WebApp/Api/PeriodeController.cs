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
    public class PeriodeController : ControllerBase {
        private IConfiguration _config;

        public PeriodeController (IConfiguration config) {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get () {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.Periode.Select ());
            }
        }

        // GET: api/Employees/5
        [HttpGet ("{id}")]
        public IActionResult GetById (int id) {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.Periode.Where (x => x.idperiode == id).FirstOrDefault ());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post ([FromBody] Periode value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    value.idperiode = db.Periode.InsertAndGetLastID (value);
                    if (value.idperiode <= 0)
                        throw new SystemException ("Data Perusahaan  Tidak Berhasil Disimpan !");
                    return Ok (value);
                }
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut ("{id}")]
        public IActionResult Put (int id, [FromBody] Periode value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    var updated = db.Periode.Update (x => new {
                        x.idperiode
                    }, value, x => x.idperiode == value.idperiode);
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
                    var deleted = db.Periode.Delete (x => x.idperiode == id);
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