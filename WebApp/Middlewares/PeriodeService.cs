using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Middlewares
{

    public interface IPeriodeService
    {
        Task<Periode> ActivePeriode();
    }
    public class PeriodeService : IPeriodeService
    {
        private ApplicationDbContext _context;

        public PeriodeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<Periode> ActivePeriode()
        {
            try
            {
                var actived = _context.Periode.Where(x => x.Status == true).FirstOrDefault();
                if (actived == null)
                    throw new SystemException("Periode Aktif Belum Ada");

                if (actived.Status==false && actived.Selesai < DateTime.Now)
                {
                    actived.Status = false;
                    _context.SaveChanges();
                }

                return Task.FromResult(actived);
            }
            catch (Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }
    }
}
