using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName ("Pointkaryawan")]
    public class Pointkaryawan
    {
        // [PrimaryKey ("idpoint")]
        // [DbColumn ("idpoint")]
        [Key]
        public int idpoint { get; set; }

        // [DbColumn ("idpointkaryawan")]
        [ForeignKey("PointKaryawan")]
        public int idpointkaryawan { get; set; }

        // [DbColumn ("idkaryawan")]
        [ForeignKey("Karyawan")]
        public int idkaryawan { get; set; }

        // [DbColumn ("point")]
        public double point { get; set; }

    }
}