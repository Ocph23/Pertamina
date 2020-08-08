using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Pointkaryawan")]
     public class Pointkaryawan {
          [PrimaryKey ("idpoint")]
          [DbColumn ("idpoint")]
          public int idpoint { get; set; }

          [DbColumn ("idpointkaryawan")]
          public int idpointkaryawan { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

          [DbColumn ("point")]
          public double point { get; set; }

     }
}