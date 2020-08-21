using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName ("Pointperusahaan")]
    public class Pointperusahaan
    {
        // [PrimaryKey ("idpointperusahaan")]
        // [DbColumn ("idpointperusahaan")]
        [Key]
        public int idpointperusahaan { get; set; }

        // [DbColumn ("idperusahaan")]
        [ForeignKey("Perusahaan")]
        public int idperusahaan { get; set; }

        [ForeignKey("Periode")]
        // [DbColumn ("idperiode")]
        public int idperiode { get; set; }

        // [DbColumn ("point")]
        public double point { get; set; }

    }
}