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
    public class LevelController : ControllerBase {
        private IConfiguration _config;

        public LevelController (IConfiguration config) {
            _config = config;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get () {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {

                var results = from a in db.Level.Select ()
                join b in db.JenisPelanggaran.Select () on a.idlevel equals b.idlevel into c

                select new Level { idlevel = a.idlevel, level = a.level, Datas = c.ToList () };

                return Ok (results.ToList ());
            }
        }

        // GET: api/Employees/5
        [HttpGet ("{id}")]
        public IActionResult GetById (int id) {
            using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                return Ok (db.Level.Where (x => x.idlevel == id).FirstOrDefault ());
            }

        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult Post ([FromBody] Level value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    value.idlevel = db.Level.InsertAndGetLastID (value);
                    if (value.idlevel <= 0)
                        throw new SystemException ("Data Perusahaan  Tidak Berhasil Disimpan !");
                    return Ok (value);
                }
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut ("{id}")]
        public IActionResult Put (int id, [FromBody] Level value) {
            try {
                using (var db = new OcphDbContext (_config.GetConnectionString ("DefaultConnection"))) {
                    var updated = db.Level.Update (x => new {
                        x.level
                    }, value, x => x.idlevel == value.idlevel);
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
                    var deleted = db.Level.Delete (x => x.idlevel == id);
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