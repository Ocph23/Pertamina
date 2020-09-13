using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class Absen
    {
        [Key]
        public int Id { get; set; }

        public AbsenType AbsenType { get; set; }

        public DateTime? Masuk { get; set; }

        public DateTime? Pulang { get; set; }

        public int KaryawanId { get; set; }
        public virtual Karyawan Karyawan { get; set; }

    }
}