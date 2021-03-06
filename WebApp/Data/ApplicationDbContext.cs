﻿using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<Karyawan>().HasIndex(u => u.KodeKaryawan).IsUnique();
        }



        public DbSet<Periode> Periode { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<DetailLevel> DetailLevels { get; set; }
        public DbSet<Karyawan> Karyawan { get; set; }
        public DbSet<Perusahaan> Perusahaan { get; set; }
        public DbSet<Absen> Absen { get; set; }
        public DbSet<Pelanggaran> Pelanggaran { get; set; }
        public DbSet<BuktiPelanggaran> BuktiPelanggaran { get; set; }
        public DbSet<Pemenang> Pemenang { get; set; }
        public DbSet<PerusahaanKaryawan> PerusahaanKaryawan { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
    }
}
