using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private IFcmService _fcm;
        private IPeriodeService _periode;
        private IConfiguration _config;
        private ApplicationDbContext _context;

        public DashboardController(IConfiguration config, ApplicationDbContext context,IFcmService fcm, IPeriodeService periode)
        {
            _fcm = fcm;
            _periode = periode;
            _config = config;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var actived = await _periode.ActivePeriode();
                if (actived == null)
                    throw new SystemException("Periode Aktif Belum Ditemukan");
                var pelanggarans = _context.Pelanggaran.Where(x => x.Tanggal >= actived.Mulai && x.Tanggal <= actived.Selesai)
                        .Include(x => x.ItemPelanggarans).ThenInclude(x => x.DetailLevel).ThenInclude(x => x.Level)
                        .Include(x => x.Perusahaan);
                var a = pelanggarans.SelectMany(x => x.ItemPelanggarans);
                var perusahaan = pelanggarans.ToList().GroupBy(x => x.PerusahaanId);
                var dataperusahaan = new List<dynamic>();
                foreach (var item in perusahaan)
                {
                    var dataitem = item.FirstOrDefault();
                    dataperusahaan.Add(new
                    {
                        namaperusahaan = dataitem.Perusahaan.Nama,
                        total = item.Count()
                    });
                }

                 var datas = new List<dynamic>();
                var group = a.ToList().GroupBy(x => x.DetailLevel.LevelId).ToList();
                foreach (var items in group)
                {
                    var dataTemp = items.FirstOrDefault();

                    dynamic data = new
                    {
                        LevelId = dataTemp.DetailLevel.LevelId,
                        Jenispelanggaran = dataTemp.DetailLevel.Nama,
                        Perusahaan = dataTemp.NilaiPerusahaan,
                        Karyawan = items.Count(),
                        // Tanggal = dataTemp.,
                        Level = dataTemp.DetailLevel.Level.Nama,
                         Today =  pelanggarans.Where  (x => x.Tanggal.Year == DateTime.Now.Year && x.Tanggal.Month == DateTime.Now.Month
                         && x.Tanggal.Day == DateTime.Now.Day).Count()
                    };
                    datas.Add(data);
                }

                var karyawan = _context.Karyawan.Find(1);
                if (karyawan != null)
                {
                    var notif = new NotificationModel {
                        Body = "Body Message", Title = "Titale", NotificationType = NotificationType.Private,
                        Created = DateTime.Now, Sender = "HRD", UserId = karyawan.UserId , Id=0
                    };
                    notif.UserId = karyawan.UserId;
                    await _fcm.SendMessagePrivate(notif, karyawan.DeviceId);

                    notif.UserId = string.Empty;
                    notif.NotificationType = NotificationType.Public;
                    await _fcm.SendMessage(notif, "all");

                }

                return Ok(new { datas = datas.ToList(), perusahaan = dataperusahaan });
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Karyawan")]
        public async Task<IActionResult> PointByKaryawanIdAsync()
        {
            try
            {
                var periode = await _periode.ActivePeriode();
                var selisih = periode.Selesai.Subtract(periode.Mulai);

                var datas = from karyawan in  _context.Karyawan.Include(x=>x.Perusahaans).ToList()
                            join b in _context.Pelanggaran.Include(x=>x.ItemPelanggarans).ToList()
                             on karyawan.Id equals b.TerlaporId into pelanggarans
                            select new { Karyawan=karyawan, Pelanggarans=pelanggarans};

                List<dynamic> list = new List<dynamic>();
                foreach (var item in datas)
                {
                    var groupDate = item.Pelanggarans.GroupBy(x => new { Tahun = x.Tanggal.Year, Month = x.Tanggal.Month, Day = x.Tanggal.Day });
                    var totalDay = (selisih.Days + 1) - groupDate.Count();
                    var score = 100 + (totalDay * 0.5);
                    double totalPelanggaran = item.Pelanggarans.SelectMany(x => x.ItemPelanggarans).Sum(x => x.NilaiKaryawan);
                    var total = score - totalPelanggaran;
                    list.Add(new { Karyawan = item.Karyawan, Score = score, Potongan = totalPelanggaran });
                }
               
                return Ok(list);
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