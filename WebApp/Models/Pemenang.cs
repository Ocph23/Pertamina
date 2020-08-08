using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Pemenang")]
     public class Pemenang {
          [PrimaryKey ("idpemenang")]
          [DbColumn ("idpemenang")]
          public int idpemenang { get; set; }

          [DbColumn ("idkaryawan")]
          public int idkaryawan { get; set; }

          [DbColumn ("idperiode")]
          public int idperiode { get; set; }

     }
}