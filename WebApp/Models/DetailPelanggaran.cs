using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class DetailPelanggaran
    {
        [Key]
        public int Id { get; set; }
        public double NilaiKaryawan { get; set; }

        public double NilaiPerusahaan { get; set; }
        public double Penambahan { get; set; }

        public int DetailLevelId { get; set; }
        public virtual DetailLevel DetailLevel { get; set; }

    }
}