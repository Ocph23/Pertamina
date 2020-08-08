using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Absen")]
     public class Absen {
          [PrimaryKey ("idabsen")]
          [DbColumn ("idabsen")]
          public int idabsen { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

     }
}