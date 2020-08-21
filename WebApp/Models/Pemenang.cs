using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName ("Pemenang")]

    public class Pemenang
    {
        [Key]
        // [PrimaryKey ("idpemenang")]
        // [DbColumn ("idpemenang")]
        public int idpemenang { get; set; }

        // [DbColumn ("idkaryawan")]
        [ForeignKey("Karyawan")]
        public int idkaryawan { get; set; }


        [ForeignKey("Periode")]
        // [DbColumn ("idperiode")]
        public int idperiode { get; set; }

    }
}