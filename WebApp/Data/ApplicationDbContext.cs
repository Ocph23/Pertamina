using System.Text;
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

        public DbSet<Periode> Periode { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Karyawan> Karyawan { get; set; }
        public DbSet<Jenispelanggaran> JenisPelanggaran { get; set; }
        public DbSet<Pointkaryawan> Pointkaryawan { get; set; }
        public DbSet<Perusahaan> Perusahaan { get; set; }
        public DbSet<Pointperusahaan> Pointperusahaan { get; set; }
        public DbSet<Absen> Absen { get; set; }
        public DbSet<Pelanggaran> Pelanggaran { get; set; }
        public DbSet<BuktiPelanggaran> BuktiPelanggaran { get; set; }
        public DbSet<Pemenang> Pemenang { get; set; }
        public DbSet<Pengaduan> Pengaduan { get; set; }
        public DbSet<Terlapor> Terlapor { get; set; }
    }
}
