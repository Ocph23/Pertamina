using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class PerusahaanKaryawan
    {
        [Key]
        public int Id { get; set; }
        public string Jabatan { get; set; }
        public DateTime MulaiKerja { get; set; }
        public DateTime? SelesaiKerja { get; set; }
        public bool Status { get; set; } = true;

        public int KaryawanId { get; set; }
        public virtual Karyawan Karyawan { get; set; }

        public int PerusahaanId { get; set; }
        public virtual Perusahaan Perusahaan { get; set; }


    }
}