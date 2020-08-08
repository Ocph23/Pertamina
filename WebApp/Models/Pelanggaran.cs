using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Pelanggaran")]
     public class Pelanggaran {
          [PrimaryKey ("idpelanggaran")]
          [DbColumn ("idpelanggaran")]
          public int idpelanggaran { get; set; }

          [DbColumn ("idjenispelanggaran")]
          public int idjenispelanggaran { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

     }
}