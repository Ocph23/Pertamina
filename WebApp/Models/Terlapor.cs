using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Terlapor")]
     public class Terlapor {
          [PrimaryKey ("idterlapor")]
          [DbColumn ("idterlapor")]
          public int idterlapor { get; set; }

          [DbColumn ("idpengaduan")]
          public int idpengaduan { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

          [DbColumn ("idjenispelanggaran")]
          public int idjenispelanggaran { get; set; }

     }
}