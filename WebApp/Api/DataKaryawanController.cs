using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using WebApp.Models;
using WebApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataKaryawanController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public DataKaryawanController(IConfiguration config, ApplicationDbContext dbcontext, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _context = dbcontext;
            _userManager = userManager;
        }

        // GET: api/Employees
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> profile()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var Karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).FirstOrDefault();

                return Ok(Karyawan);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("absen")]
        public async Task<IActionResult> absen()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).FirstOrDefault();
                var absens = _context.Absen.Include(x => x.Karyawan).Where(x => x.Karyawan.Id == karyawan.Id).ToList();
                return Ok(absens);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("pelanggaran")]
        public async Task<IActionResult> pelanggaran()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).FirstOrDefault();
                    var pelanggarans = _context.Pelanggaran.Where(x => x.Terlapor.Id == karyawan.Id)
                        .Include(x => x.Terlapor)
                        .Include(x => x.Files)
                        .Include(x => x.ItemPelanggarans).ThenInclude(x => x.DetailLevel).ThenInclude(x => x.Level);
                    return Ok(pelanggarans.ToList());
                }

                throw new SystemException("Anda Tidak Memiliki Akses");

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}