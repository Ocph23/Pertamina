
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApp.Middlewares;

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
        [Authorize]
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
                var karyawan = _context.Karyawan.Where(x => x.UserId == user.Id).FirstOrDefault();
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
    }
}