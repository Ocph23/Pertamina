using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Perusahaan
    {
        [Key]
        public int Id { get; set; }

        public string Nama { get; set; }

        public string Alamat { get; set; }

        public string Direktur { get; set; }

        public string Kontak { get; set; }

        public string Email { get; set; }

        public string Logo { get; set; }

        [NotMapped]
        public byte[] DataPhoto { get; set; }

        public virtual ICollection<PerusahaanKaryawan> PerusahaansKaryawan { get; set; }

        public virtual ICollection<Pelanggaran> Pelanggarans { get; set; }

    }


}