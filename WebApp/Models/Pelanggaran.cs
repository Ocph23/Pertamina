using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class Pelanggaran
    {
        [Key]
        public int Id { get; set; }
        public double NilaiKaryawan { get; set; }

        public double NilaiPerusahaan { get; set; }

        public DateTime? Tanggal { get; set; }

        public StatusPelanggaran Status { get; set; }

        public int JenisPelanggaranId { get; set; }
        public JenisPelanggaran Jenispelanggaran { get; set; }

        public int KaryawanId { get; set; }
        public virtual Karyawan DataKaryawan { get; set; }

        public int PerusahaanId { get; set; }
        public virtual Perusahaan DataPerusahaan { get; set; }
        
        public ICollection<BuktiPelanggaran> Files { get; set; }
        
        [NotMapped]
        public virtual Level Level { get; set; }

    }
}