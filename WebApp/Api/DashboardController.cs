using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;


namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]

    public class DashboardController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public DashboardController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // var actived = _context.Periode.Where(x => x.Status == true).FirstOrDefault();
                // if (actived == null)
                //     throw new SystemException("Periode Aktif Belum Ditemukan");
                // var pelanggarans =


                //  _context.Pelanggaran.Include(x => x.ItemPelanggaran)
                //                   .Where(x => x.Tanggal >= actived.Mulai && x.Tanggal <= actived.Selesai)
                //                    select new DashboardJenis
                //                    {
                //                        Level = b.Name,
                //                        LevelId = b.Id,
                //                        JenisPelanggaran = a.Jenispelanggaran.Nama,
                //                        Perusahaan = a.NilaiPerusahaan,
                //                        Karyawan = a.NilaiKaryawan,
                //                        Tanggal = a.Tanggal.Value
                //                    };

                // var datas = new List<dynamic>();

                // var group = pelanggarans.ToList().GroupBy(x => x.LevelId).ToList();
                // foreach (var items in group)
                // {
                //     var dataTemp = items.FirstOrDefault();

                //     dynamic data = new
                //     {
                //         LevelId = dataTemp.LevelId,
                //         Jenispelanggaran = dataTemp.JenisPelanggaran,
                //         Perusahaan = dataTemp.Perusahaan,
                //         Karyawan = items.Count(),
                //         Tanggal = dataTemp.Tanggal,
                //         Level = dataTemp.Level,
                //         Today = items.Where(x => x.Tanggal.Year == DateTime.Now.Year && x.Tanggal.Month == DateTime.Now.Month && x.Tanggal.Day == DateTime.Now.Day).Count()
                //     };
                //     datas.Add(data);
                // }
                return Ok();
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
        public int LevelId { get; set; }
        public string JenisPelanggaran { get; set; }
        public double Karyawan { get; set; }
        public double Perusahaan { get; set; }
        public DateTime Tanggal { get; set; }
    }
}