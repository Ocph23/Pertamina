using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Middlewares
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(UserLogin model);
        Task<IdentityUser> GetById(string id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly AppSettings _appSettings;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserService(IOptions<AppSettings> appSettings, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = dbcontext;

            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(UserLogin model)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.UserName == model.Username || x.Email == model.Username);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var token = await generateJwtToken(user);

                        return new AuthenticateResponse(user, token);
                    }
                    throw new SystemException($"Your Account {result.ToString()}");
                }
                throw new SystemException("You Not Have Access");

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public async Task<IdentityUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        // helper methods

        private async Task<string> generateJwtToken(IdentityUser user)
        {
            // generate token that is valid for 7 days

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.UserName),
                    new Claim("role", roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}