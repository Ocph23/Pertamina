using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;

namespace PertaminaWebUnitTesting
{
    public class AbsenTest
    {
        private List<Absen> listAbsen;

        [SetUp]
        public void SetUp()
        {
            listAbsen = new List<Absen>() { 
                new Absen{ Id=1, KaryawanId=1, AbsenType= AbsenType.Kerja, Masuk=new DateTime(2020,9,16,7,10,0) },
                new Absen{ Id=1, KaryawanId=1, AbsenType= AbsenType.Lembur, Masuk=new DateTime(2020,9,16,15,10,0) },
                new Absen{ Id=1, KaryawanId=1, AbsenType= AbsenType.Kerja, Masuk=new DateTime(2020,9,15,7,11,0) },
                new Absen{ Id=1,KaryawanId=1, AbsenType= AbsenType.Kerja, Masuk=new DateTime(2020,9,14,7,20,0) }
            };
        }

        [Test]
        public  void GetAbsenKerja_Today_MustBe_Null()
        {
            var now = new DateTime(2020, 9, 17, 7, 10, 0);
            var result = this.listAbsen.Where(x =>x.KaryawanId==1 && x.AbsenType == AbsenType.Kerja && x.Masuk.Value.Year == now.Year
                        && x.Masuk.Value.Month == now.Month && x.Masuk.Value.Day == now.Day
                        ).FirstOrDefault();  

            Assert.IsNull(result);
        }

        [Test]
        public void GetAbsenKerja_Today_MustBe_NotNull()
        {
            var now = new DateTime(2020, 9, 16, 7, 10, 0);
            var result = this.listAbsen.Where(x => x.KaryawanId == 1 && x.AbsenType == AbsenType.Kerja && x.Masuk.Value.Year == now.Year
                        && x.Masuk.Value.Month == now.Month && x.Masuk.Value.Day == now.Day
                        ).FirstOrDefault();

            Assert.NotNull(result);
            Assert.AreEqual(result.AbsenType, AbsenType.Kerja);
        }

        [Test]
        public void GetAbsenLembur_Today_MustBe_Null()
        {
            var now = new DateTime(2020, 9, 17, 7, 10, 0);
            var result = this.listAbsen.Where(x => x.KaryawanId == 1 && x.AbsenType == AbsenType.Lembur&& x.Masuk.Value.Year == now.Year
                        && x.Masuk.Value.Month == now.Month && x.Masuk.Value.Day == now.Day
                        ).FirstOrDefault();
            Assert.IsNull(result);
        }

        [Test]
        public void GetAbsenLembur_Today_MustBe_NotNull()
        {
            var now = new DateTime(2020, 9, 16, 7, 10, 0);
            var result = this.listAbsen.Where(x => x.KaryawanId == 1 && x.AbsenType == AbsenType.Lembur && x.Masuk.Value.Year == now.Year
                        && x.Masuk.Value.Month == now.Month && x.Masuk.Value.Day == now.Day
                        ).FirstOrDefault();

            Assert.NotNull(result);
            Assert.AreEqual(result.AbsenType,AbsenType.Lembur);
        }



        [Test]
        public async Task Absen_IsNew_MustBeSuccessAddIn()
        {
            var type = AbsenType.Kerja;
            Absen absenToday = null;
            var result = await absen(absenToday, 1, type);                                    
            Assert.NotNull(result);
        }

        [Test]
        public async Task Absen_AlradyIn_MustBeSuccessAddOut()
        {
            var now = new DateTime(2020, 9, 16, 8, 10, 0);
            var type = AbsenType.Kerja;
           var  absenToday = new Absen { KaryawanId = 1, AbsenType = type, Masuk = new DateTime(2020, 9, 16, 7, 0, 0) };
            var result = await absen(absenToday, 1, type);
            Assert.NotNull(result.Pulang);
        }

        [Test]                               
        public  void Absen_AlradyIn_LestThen_1HourAgo_MustBe_throwExecption()
        {
            var now = new DateTime(2020, 9, 16, 7, 0, 0);
            var type = AbsenType.Kerja;
            var absenToday = new Absen { KaryawanId = 1, AbsenType = type, Masuk = new DateTime(2020, 9, 16, 7, 30, 0)};
            Assert.Throws(typeof(SystemException), new TestDelegate(()=> absen(absenToday, 1, type)));
        }

        [Test]
        public void Absen_AlradyIn_And_AlradyOut_MustBe_throwExecption()
        {
            var now = new DateTime(2020, 9, 16, 7, 0, 0);
            var type = AbsenType.Kerja;
            var absenToday = new Absen { KaryawanId = 1, AbsenType = type, Pulang  = new DateTime(2020, 9, 16, 7, 30, 0), Masuk = new DateTime(2020, 9, 16, 7, 30, 0) };
            Assert.Throws(typeof(SystemException), new TestDelegate(() => absen(absenToday, 1, type)));
        }

        private Task<Absen> absen(Absen absenToday, int karyawanId, AbsenType type)
        {
            var saveResult = 0;
            string message=string.Empty;
            if (absenToday == null)
            {
                absenToday = new Absen { KaryawanId = karyawanId, AbsenType = type, Masuk = DateTime.Now };
                //Add to _db.Absen.Add(absenToday);
                saveResult = 1;// await _db.SaveChangesAsync();
                return saveResult > 0 ? Task.FromResult(absenToday) : throw new SystemException("Absen Gagal!, Coba Ulangi Lagi");
            }

            if(absenToday.Masuk!=null && absenToday.Pulang == null)
            {
                var now = new DateTime(2020, 9, 16, 8, 0, 0);
                if(now.Subtract(absenToday.Masuk.Value) < new TimeSpan(1, 0, 0))
                {
                    throw new SystemException($"Anda Sudah Absen Masuk {type} Hari Ini !");
                }
                else
                {
                    absenToday.Pulang = now;
                    saveResult = 1;// await _db.SaveChangesAsync();
                    return saveResult > 0 ? Task.FromResult(absenToday) : throw new SystemException("Absen Gagal!, Coba Ulangi Lagi");
                }
            }
            throw new SystemException($"Anda Sudah Absen Pulang {type} Hari Ini !");
        }

    }
}
