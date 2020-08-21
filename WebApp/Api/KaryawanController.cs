using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaryawanController : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;

        public KaryawanController(IConfiguration config, SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, ApplicationDbContext dbcontext,
            IEmailSender emailSender)
        {
            _config = config;
            _userManager = userManager;
            _context = dbcontext;
        }

        // GET: api/Employees
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get()
        {
            try
            {

                var result = from a in _context.Karyawan
                             join b in _context.Perusahaan on a.idperusahaan equals b.idperusahaan
                             select new Karyawan
                             {
                                 idkaryawan = a.idkaryawan,
                                 kodekaryawan = a.kodekaryawan,
                                 namakaryawan = a.namakaryawan,
                                 perusahaan = b,
                                 jabatan = a.jabatan,
                                 kontak = a.kontak,
                                 email = a.email,
                                 alamat = a.alamat,
                                 photo = a.photo
                             };


                return Ok(result.ToList());
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
                var result = from a in _context.Karyawan
                             join b in _context.Perusahaan on a.idperusahaan equals b.idperusahaan
                             select new Karyawan
                             {
                                 idkaryawan = a.idkaryawan,
                                 kodekaryawan = a.kodekaryawan,
                                 namakaryawan = a.namakaryawan,
                                 perusahaan = b,
                                 jabatan = a.jabatan,
                                 kontak = a.kontak,
                                 email = a.email,
                                 alamat = a.alamat,
                                 photo = a.photo
                             };
                var karyawan = result.FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.kodekaryawan);
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
                var karyawan = _context.Karyawan.Where(x => x.idkaryawan == id).FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.kodekaryawan);
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
            value.idperusahaan = value.perusahaan.idperusahaan;
            var user = new IdentityUser { UserName = value.kodekaryawan };
            try
            {
                var result = await _userManager.CreateAsync(user, "Id" + value.kodekaryawan + "#");
                if (result.Succeeded)
                {
                    value.userid = user.Id;
                    if (value.DataPhoto != null && value.DataPhoto.Length > 0)
                    {
                        value.photo = Helpers.CreateFileName("image");
                        System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.photo, Helpers.CreateThumb(value.DataPhoto));
                    }
                    value.perusahaan = null;
                    _context.Karyawan.Add(value);
                    _context.SaveChanges();
                    if (value.idkaryawan <= 0)
                        throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");
                    var addUserResult = await _userManager.AddToRoleAsync(user, "Karyawan");
                    return Ok(value);
                }
                throw new SystemException("User Tidak Berhasil Dibuat");
            }
            catch (System.Exception ex)
            {
                if (user.Id != string.Empty)
                {
                    await _userManager.DeleteAsync(user);
                }
                return BadRequest(ex.Message);
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
                    value.photo = Helpers.CreateFileName("image");
                    System.IO.File.WriteAllBytes(Helpers.ProfilePath + value.photo, Helpers.CreateThumb(value.DataPhoto));
                }
                var data = _context.Karyawan.Where(x => x.idkaryawan == value.idkaryawan).FirstOrDefault();
                data.idperusahaan = value.idperusahaan;
                data.alamat = value.alamat;
                data.jabatan = value.jabatan;
                data.kontak = value.kontak;
                data.namakaryawan = value.namakaryawan;
                data.photo = value.photo;
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
                var data = _context.Karyawan.Where(x => x.idkaryawan == id).FirstOrDefault();
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
                var karyawan = _context.Karyawan.Where(x => x.idkaryawan == value.IdKaryawan).FirstOrDefault();
                var user = await _userManager.FindByNameAsync(karyawan.kodekaryawan);
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