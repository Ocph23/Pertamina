using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;

namespace WebApp.Controllers
{

    [Microsoft.AspNetCore.Authorization.Authorize]
    public class UndianController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPeriodeService _periodeService;

        public UndianController(ILogger<HomeController> logger, ApplicationDbContext context, IPeriodeService periodeService)
        {
            _logger = logger;
            _context = context;
            _periodeService = periodeService;
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin, administrator")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var active = await _periodeService.ActivePeriode();
                if (active == null)
                    throw new SystemException("Periode Active Belum Ada");
                if (active.Status==false)
                    throw new SystemException("Periode Active Belum Ada");

                var pemenang = _context.Pemenang.Where(x => x.PeriodeId == active.Id)
                    .Include(x=>x.Karyawan).FirstOrDefault();
                if (pemenang != null)
                {
                    ViewBag.Pemenang = pemenang;
                }

                return View();
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}