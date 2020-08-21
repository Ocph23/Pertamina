using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IConfiguration _config;

        public DashboardController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Employees


        // GET: api/Employees/5
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var active = db.Query<Periode>($"select * from periode where status='true'").FirstOrDefault();
                    if (active != null)
                    {
                        var sql = $@"SELECT
                        `level`.`level`,
                        `level`.`idlevel`,
                        `jenispelanggaran`.`jenispelanggaran`,
                        `pelanggaran`.`karyawan`,
                        `pelanggaran`.`perusahaan`,
                        `pelanggaran`.`tanggal`
                        FROM
                        `level`
                        LEFT JOIN `jenispelanggaran`
                        ON `level`.`idlevel` = `jenispelanggaran`.`idlevel`
                        LEFT JOIN `pelanggaran` ON `jenispelanggaran`.`idjenispelanggaran` =
                        `pelanggaran`.`idjenispelanggaran` where tanggal is null or 
                        (tanggal >= '{active.tanggalmulai.ToString("yyyy-MM-dd HH:mm:ss")}' and 
                        tanggal <= '{active.tanggalselesai.ToString("yyyy-MM-dd HH:mm:ss")}')";

                        List<dynamic> datas = new List<dynamic>();
                        List<dynamic> todays = new List<dynamic>();
                        var result = db.Query<DashboardJenis>(sql);
                        var group = result.GroupBy(x => x.IdLevel).ToList();
                        foreach (var items in group)
                        {
                            var dataTemp = items.FirstOrDefault();

                            dynamic data = new
                            {
                                IdLevel = dataTemp.IdLevel,
                                Jenispelanggaran = dataTemp.JenisPelanggaran,
                                Perusahaan = dataTemp.Perusahaan,
                                Karyawan = items.Count(),
                                Tanggal = dataTemp.Tanggal,
                                Level = dataTemp.Level,
                                Today = items.Where(x => x.Tanggal.Year == DateTime.Now.Year).Count()
                            };
                            datas.Add(data);
                        }

                        return Ok(new { Datas = datas });
                    }

                    throw new SystemException("Periode Aktif Belum Ditemukan");


                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }


    public class DashboardJenis
    {
        public string Level { get; set; }
        public string IdLevel { get; set; }
        public string JenisPelanggaran { get; set; }
        public double Karyawan { get; set; }
        public double Perusahaan { get; set; }
        public DateTime Tanggal { get; set; }
    }
}