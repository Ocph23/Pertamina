﻿using System;
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
using WebApp.Models;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaryawanController : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<IdentityUser> _userManager;
        public KaryawanController(IConfiguration config, SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _config = config;
            _userManager = userManager;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                 var result = from a in db.Karyawan.Select()
                             join b in db.Perusahaan.Select() on a.idperusahaan equals b.idperusahaan
                             select new Karyawan { idkaryawan = a.idkaryawan,kodekaryawan=a.kodekaryawan, namakaryawan=a.namakaryawan, perusahaan=b, 
                               jabatan=a.jabatan, kontak=a.kontak, email=a.email, alamat=a.alamat
                             };
                                
                return Ok(result.ToList());
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
            {
                var result = from a in db.Karyawan.Where(x=>x.idkaryawan==id)
                             join b in db.Perusahaan.Select() on a.idperusahaan equals b.idperusahaan
                             select new Karyawan { idkaryawan = a.idkaryawan, kodekaryawan=a.kodekaryawan, namakaryawan=a.namakaryawan, perusahaan=b, 
                               jabatan=a.jabatan, kontak=a.kontak, email=a.email, alamat=a.alamat
                             };
                                
                return Ok(result.FirstOrDefault());
            }
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Karyawan value)
        {
            try
            {
                value.idperusahaan = value.perusahaan.idperusahaan;
                var user = new IdentityUser { UserName = value.kodekaryawan };
                var result = await _userManager.CreateAsync(user, "Id" + value.kodekaryawan + "#");
                if (result.Succeeded)
                {
                    value.userid = user.Id;
                    using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                    {
                        value.idkaryawan = db.Karyawan.InsertAndGetLastID(value);
                        if (value.idkaryawan <= 0)
                            throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");
                        var addUserResult = await _userManager.AddToRoleAsync(user, "Karyawan");
                        return Ok(value);
                    }
                }
                throw new SystemException("User Tidak Berhasil Dibuat");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Karyawan value)
        {
            try
            {
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var updated = db.Karyawan.Update(x => new { x.idperusahaan, x.jabatan, x.kontak, x.namakaryawan, x.photo }, value, x => x.idkaryawan == value.idkaryawan);
                    if (!updated)
                        throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");

                    return Ok(value);
                }
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
                using (var db = new OcphDbContext(_config.GetConnectionString("DefaultConnection")))
                {
                    var deleted = db.Karyawan.Delete(x => x.idkaryawan == id);
                    if (!deleted)
                        throw new SystemException("Data Karyawan  Tidak Berhasil Disimpan !");
                    return Ok(true);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}