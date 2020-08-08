using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Pointperusahaan")]
     public class Pointperusahaan {
          [PrimaryKey ("idpointperusahaan")]
          [DbColumn ("idpointperusahaan")]
          public int idpointperusahaan { get; set; }

          [DbColumn ("idperusahaan")]
          public int idperusahaan { get; set; }

          [DbColumn ("idperiode")]
          public int idperiode { get; set; }

          [DbColumn ("point")]
          public double point { get; set; }

     }
}