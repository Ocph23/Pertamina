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
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(IConfiguration config, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        // GET: api/Employees


        // GET: api/Employees/5
        [HttpGet("profile")]
        public async Task<IActionResult> profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            user.PasswordHash = null;
            user.SecurityStamp = null;
            user.ConcurrencyStamp = null;
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                var nama = user.UserName;
                var karyawan = db.Karyawan.Where(x => x.userid == user.Id).FirstOrDefault();
                if (karyawan != null)
                {
                    nama = karyawan.namakaryawan;

                }

                return Ok(new { UserName = nama, User = user, Roles = roles, Karyawan = karyawan });
            }




        }


    }
}