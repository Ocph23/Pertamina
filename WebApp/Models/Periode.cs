using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName("Periode")]
    public class Periode
    {
        // [PrimaryKey("idperiode")]
        // [DbColumn("idperiode")]
        [Key]
        public int idperiode { get; set; }

        // [DbColumn("tanggalmulai")]
        public DateTime tanggalmulai { get; set; }

        // [DbColumn("tanggalselesai")]
        public DateTime tanggalselesai { get; set; }

        // [DbColumn("tanggalundian")]
        public DateTime tanggalundian { get; set; }

        // [DbColumn("status")]
        public bool status { get; set; }

    }
}