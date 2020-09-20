using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Middlewares
{

    public interface IAbsenModel
    {
        Task<Absen> absen(int karyawanID, AbsenType type);
    }


    public class AbsenModel : IAbsenModel
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;

        public AbsenModel()
        {
        }

        public AbsenModel(IOptions<AppSettings> appSettings, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _db = dbcontext;

        }

        public async Task<Absen> absen(int karyawanId, AbsenType type)
        {
            var now = DateTime.Now;
            var absenToday = _db.Absen.Where(x => x.KaryawanId == karyawanId && x.AbsenType == type &&
            (x.Masuk.Value.Year == now.Year && x.Masuk.Value.Month == now.Month && x.Masuk.Value.Day == now.Day)).FirstOrDefault();
            int saveResult = 0;
            if (absenToday == null)
            {
                absenToday = new Absen { KaryawanId = karyawanId, AbsenType = type, Masuk = now };
                _db.Absen.Add(absenToday);
                saveResult = await _db.SaveChangesAsync();
            }
            else if (absenToday.Pulang != null)
            {
                throw new SystemException($"Maaf Anda Sudah Absen {type.ToString()} Hari Ini ");
            }
            else
            {
                absenToday.Pulang = now;
                saveResult = await _db.SaveChangesAsync();
            }

            return saveResult > 0 ? absenToday : throw new SystemException("Maaf Terjadi Kesalahan Coba Ulangi Lagi !");

        }

        //setting
        //  private TimeSpan jamMasuk = new TimeSpan(7, 0, 0);
        // private int maxTerlambat = 15;
    }
}