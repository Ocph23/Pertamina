using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Middlewares;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KaryawanController : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        private IEmailSender _emailSender;

        public KaryawanController(IConfiguration config, SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, ApplicationDbContext dbcontext,
            IEmailSender emailSender)
        {
            _config = config;
            _userManager = userManager;
            _context = dbcontext;
            _emailSender = emailSender;
        }

        // GET: api/Employees
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get()
        {
            try
            {

                var active = _context.Periode.Where(x => x.Status == true).FirstOrDefault();

                if (active != null)
                {
                    var result = _context.Karyawan
                       .Include(x => x.Perusahaans)
                    .ThenInclude(x => x.Perusahaan)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(z => z.DataPerusahaan)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x.Files)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x.Jenispelanggaran).ThenInclude(x => x.Level);

                    return Ok(result.ToList());
                }
                throw new SystemException("Periode Aktif Belum Ada !");
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var karyawans = _context.Karyawan.Where(x => x.Id == id)
                .Include(x => x.Perusahaans)
                    .ThenInclude(x => x.Perusahaan)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(z => z.DataPerusahaan)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x.Files)
                .Include(x => x.Pelanggarans)
                    .ThenInclude(x => x.Jenispelanggaran).ThenInclude(x => x.Level);

                var karyawan = karyawans.FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.KodeKaryawan);
                karyawan.Roles = await _userManager.GetRolesAsync(user) as List<string>;


                return Ok(karyawan);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRoles(int id)
        {
            try
            {
                var karyawan = _context.Karyawan.Where(x => x.Id == id).FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.KodeKaryawan);
                karyawan.Roles = await _userManager.GetRolesAsync(user) as List<string>;
                return Ok(karyawan.Roles.ToList());
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Karyawan value)
        {
            var user = new IdentityUser { UserName = value.KodeKaryawan };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var password = "Pwd" + value.KodeKaryawan + "#";
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userid = user.Id, code = code },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. <hr/> <strong>Password : {password}</strong> ");
                        }
                        else
                        {
                            await _userManager.ConfirmEmailAsync(user, code);
                        }

                        value.UserId = user.Id;
                        if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                        {
                            value.Photo = Helpers.CreateFileName("image");
                            System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.Photo, Helpers.CreateThumb(value.DataPhoto));
                        }

                        value.Perusahaans.Add(value.PerusahaanKaryawan);
                        _context.Karyawan.Add(value);
                        _context.SaveChanges();
                        if (value.Id <= 0)
                            throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");
                        var addUserResult = await _userManager.AddToRoleAsync(user, "Karyawan");
                        transaction.Commit();
                        return Ok(value);
                    }
                    throw new SystemException("User Tidak Berhasil Dibuat");
                }
                catch (System.Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Karyawan value)
        {
            try
            {
                if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                {
                    value.Photo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.Photo, Helpers.CreateThumb(value.DataPhoto));
                }
                var data = _context.Karyawan.Where(x => x.Id == value.Id).Include(x => x.Perusahaans).FirstOrDefault();

                if (data.PerusahaanKaryawan != null && data.PerusahaanKaryawan.PerusahaanId != value.PerusahaanKaryawan.PerusahaanId)
                {
                    value.PerusahaanKaryawan.Id = 0;
                    data.Status = false;
                    data.Perusahaans.Add(value.PerusahaanKaryawan);
                }
                data.PerusahaanKaryawan.SelesaiKerja = DateTime.Now;
                data.Alamat = value.Alamat;
                data.Kontak = value.Kontak;
                data.NamaKaryawan = value.NamaKaryawan;
                data.Photo = value.Photo;
                _context.SaveChanges();
                return Ok(value);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var data = _context.Karyawan.Where(x => x.Id == id).FirstOrDefault();
                _context.Karyawan.Remove(data);
                var deleted = _context.SaveChanges();
                if (deleted <= 0)
                    throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("changerole")]
        public async Task<IActionResult> changerole([FromBody] RoleModel value)
        {

            try
            {
                var karyawan = _context.Karyawan.Where(x => x.Id == value.IdKaryawan).FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.KodeKaryawan);
                if (value.Checked)
                {
                    var result = await _userManager.AddToRoleAsync(user, value.Role);

                    if (result.Succeeded)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        throw new SystemException("Gagal Diubah !");
                    }
                }
                else
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, value.Role);

                    if (result.Succeeded)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        throw new SystemException("Gagal Diubah !");
                    }
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
    }



    public class RoleModel
    {
        public int IdKaryawan { get; set; }
        public string Role { get; set; }
        public bool Checked { get; set; }
    }
}