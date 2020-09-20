
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApp.Middlewares;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IUserService _userService;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _config;

        public UserController(IConfiguration config,
                IUserService userService,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _context = dbcontext;
            _userService = userService;
        }

        // GET: api/Employees


        // GET: api/Employees/5
        [ApiAuthorize]
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
                var karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).Include(x => x.Perusahaans)
                    .ThenInclude(x => x.Perusahaan).FirstOrDefault();
                if (karyawan != null)
                {
                    nama = karyawan.NamaKaryawan;

                }

                return Ok(new { UserName = nama, User = user, Roles = roles, Karyawan = karyawan });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("profilebyproviderid")]
        public async Task<IActionResult> ProfilebyproviderId(string Id, string provider)
        {
            try
            {
                var userProvider = _context.UserLogins.Where(x => x.ProviderKey == Id && x.LoginProvider == provider).FirstOrDefault();

                if (userProvider == null)
                {
                    throw new SystemException("Anda Tidak Memiliki Akses !");
                }
                
                var user = await _userManager.FindByIdAsync(userProvider.UserId);
                if (user == null)
                    throw new SystemException("Anda Tidak Memiliki Akses !");

                var roles = await _userManager.GetRolesAsync(user);
                user.PasswordHash = null;
                user.SecurityStamp = null;
                user.ConcurrencyStamp = null;
                var nama = user.UserName;
                var karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).Include(x => x.Perusahaans)
                    .ThenInclude(x => x.Perusahaan).FirstOrDefault();
                if (karyawan != null)
                {
                    nama = karyawan.NamaKaryawan;

                }
                var token = await _userService.AuthenticateUSerProvider(user);
                return Ok(new { UserName = nama, User = user, Roles = roles, Karyawan = karyawan, Token = token });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            try
            {
                var response = await _userService.Authenticate(user);
                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [ApiAuthorize] 
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if(user.Id!=model.UserId)
                {
                    throw new SystemException("Anda Tidak Memiliki Access Untuk Mengubah Password");
                }

               var result =   await  _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(true);
                }
                throw new SystemException("Ubah Password Tidak Berhasil !, \r\n Note : Password Harus Mengandung Angka, Huruf Besar, Huruf Kecil dan Karakter Khusus");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ApiAuthorize]
        [HttpGet("JointExternalUser")]
        public async Task<IActionResult> JointExternalUser(string key, string provider)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                UserLoginInfo userLogin = new UserLoginInfo(provider, key, provider);
                var result = await _userManager.AddLoginAsync(user, userLogin);
                if (result.Succeeded)
                {
                    return Ok(true);
                }
                throw new SystemException("Account Tidak Berhasil Dihubungkan !");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ApiAuthorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if(user!=null)
                {
                    IEnumerable datas = _context.Notifications.Where(x => x.NotificationType == NotificationType.Public || x.UserId == user.Id).OrderByDescending(x=>x.Created).ToList();
                    return Ok(datas);
                }
                throw new SystemException("Data Tidak Ada !");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}