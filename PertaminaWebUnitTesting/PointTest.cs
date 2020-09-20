using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace PertaminaWebUnitTesting
{
   public class PointTest
    {
        private List<Pelanggaran> datas;
        private Periode periode;

        [SetUp]
        public void Setup()
        {
            datas = new List<Pelanggaran>() {
                new Pelanggaran{ Tanggal= new DateTime(2020,9,1),
                   ItemPelanggarans= new List<DetailPelanggaran>(){
                      new DetailPelanggaran{ NilaiKaryawan=0.5, NilaiPerusahaan=.1, Penambahan=.5 },
                      new DetailPelanggaran{ NilaiKaryawan=1.5, NilaiPerusahaan=1.1, Penambahan=1.5}
                   }
                },
                new Pelanggaran{ Tanggal= new DateTime(2020,9,2),
                   ItemPelanggarans= new List<DetailPelanggaran>(){
                      new DetailPelanggaran{ NilaiKaryawan=0.1, NilaiPerusahaan=.1, Penambahan=.1 },
                      new DetailPelanggaran{ NilaiKaryawan=1.2, NilaiPerusahaan=1.3, Penambahan=1}
                   }
                },
            };

            periode = new Periode { Id = 1, Mulai = new DateTime(2020, 9, 1), Selesai = new DateTime(2020, 9, 10), Status = true };

        }


        [Test]
        public void Substract_StartDate_With_EndDate_Must_10()
        {
            var periode = new Periode { Id = 1, Mulai = new DateTime(2020, 9, 1),
                Selesai = new DateTime(2020, 9, 10), Status = true };

            var selisih = periode.Selesai.Subtract(periode.Mulai);
            Assert.AreEqual(selisih.Days, 9);
        }

        [Test]
        public void CountTotalDay_OfPelanggaran_Must_2()
        {
            var groupDate = datas.GroupBy(x => new { Tahun = x.Tanggal.Year, Month = x.Tanggal.Month, Day = x.Tanggal.Day });
            Assert.AreEqual(groupDate.Count(), 2);
        }


        [Test]
        public void CalculatePoint_Must_3Point3()
        {
           
            var selisih = periode.Selesai.Subtract(periode.Mulai);
            var groupDate= datas.GroupBy(x => new { Tahun = x.Tanggal.Year, Month = x.Tanggal.Month, Day = x.Tanggal.Day });
            var totalDay = (selisih.Days+1)-groupDate.Count();
            var score = 100 + (totalDay * 0.5);
            double totalPelanggaran= datas.SelectMany(x => x.ItemPelanggarans).Sum(x=>x.NilaiKaryawan);
            double mustResult = 3.3;            
            Assert.AreEqual(mustResult.ToString(), totalPelanggaran.ToString());
        }

        [Test]
        public void PointPelanggaran_must_3_3()
        {
            double totalPelanggaran = datas.SelectMany(x => x.ItemPelanggarans).Sum(x => x.NilaiKaryawan);
            double mustResult = 3.3;
            Assert.AreEqual(mustResult.ToString(), totalPelanggaran.ToString());
        }


        [Test]
        public void TotalPoint_Must_()
        {

            var selisih = periode.Selesai.Subtract(periode.Mulai);
            var groupDate = datas.GroupBy(x => new { Tahun = x.Tanggal.Year, Month = x.Tanggal.Month, Day = x.Tanggal.Day });
            var totalDay = (selisih.Days + 1) - groupDate.Count();
            var score = 100 + (totalDay * 0.5);
            double totalPelanggaran = datas.SelectMany(x => x.ItemPelanggarans).Sum(x => x.NilaiKaryawan);
            var total = score - totalPelanggaran;
            Assert.AreEqual(100.7, total);
        }

        [Test]
        public void TotalPoint_NotHave_Pelanggaran_Must_()
        {
            datas = new List<Pelanggaran>() {
            };

            var selisih = periode.Selesai.Subtract(periode.Mulai);
            var groupDate = datas.GroupBy(x => new { Tahun = x.Tanggal.Year, Month = x.Tanggal.Month, Day = x.Tanggal.Day });
            var totalDay = (selisih.Days + 1) - groupDate.Count();
            var score = 100 + (totalDay * 0.5);
            double totalPelanggaran = datas.SelectMany(x => x.ItemPelanggarans).Sum(x => x.NilaiKaryawan);
            var total = score - totalPelanggaran;
            Assert.AreEqual(100.7, total);
        }
    }
}
