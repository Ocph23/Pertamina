using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Pengaduan")]
     public class Pengaduan {
          [PrimaryKey ("idpengaduan")]
          [DbColumn ("idpengaduan")]
          public int idpengaduan { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

          [DbColumn ("jenis")]
          public string jenis { get; set; }

          [DbColumn ("idlistpengaduan")]
          public int idlistpengaduan { get; set; }

     }
}