using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName ("Pengaduan")]
    public class Pengaduan
    {
        [Key]
        // [PrimaryKey ("idpengaduan")]
        // [DbColumn ("idpengaduan")]
        public int idpengaduan { get; set; }

        // [DbColumn ("idkaryawan")]
        [ForeignKey("Pengaduan")]
        public int idkaryawan { get; set; }

        // [DbColumn ("jenis")]
        public string jenis { get; set; }

        // [DbColumn ("idlistpengaduan")]
        public int idlistpengaduan { get; set; }

    }
}