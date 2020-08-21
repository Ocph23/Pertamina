using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(IConfiguration config, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = dbcontext;
        }

        // GET: api/Employees


        // GET: api/Employees/5
        [HttpGet("profile")]
        public async Task<IActionResult> profile()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                user.PasswordHash = null;
                user.SecurityStamp = null;
                user.ConcurrencyStamp = null;
                var nama = user.UserName;
                var karyawan = _context.Karyawan.Where(x => x.userid == user.Id).FirstOrDefault();
                if (karyawan != null)
                {
                    nama = karyawan.namakaryawan;

                }

                return Ok(new { UserName = nama, User = user, Roles = roles, Karyawan = karyawan });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


    }
}