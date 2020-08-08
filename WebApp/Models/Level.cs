using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models {
     [TableName ("Level")]
     public class Level {
          [PrimaryKey ("idlevel")]
          [DbColumn ("idlevel")]
          public int idlevel { get; set; }

          [DbColumn ("level")]
          public string level { get; set; }

     }
}