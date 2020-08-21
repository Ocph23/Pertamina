using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Ocph.DAL;

namespace WebApp.Models
{
    // [TableName("Jenispelanggaran")]
    public class Jenispelanggaran
    {
        //   [PrimaryKey ("idjenispelanggaran")]
        // [DbColumn ("idjenispelanggaran")]

        [Key]
        public int idjenispelanggaran { get; set; }

        //[DbColumn("idlevel")]
        public int idlevel { get; set; }

        // [DbColumn("jenispelanggaran")]
        public string jenispelanggaran { get; set; }

        //[DbColumn("pengurangankaryawan")]
        public double pengurangankaryawan { get; set; }

        //[DbColumn("penguranganperusahaan")]
        public double penguranganperusahaan { get; set; }

        //[DbColumn("penambahanpoint")]
        public double penambahanpoint { get; set; }




    }
}