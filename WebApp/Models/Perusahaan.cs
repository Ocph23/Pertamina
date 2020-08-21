using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // [TableName("Perusahaan")]
    public class Perusahaan
    {
        // [PrimaryKey("idperusahaan")]
        // [DbColumn("idperusahaan")]
        [Key]
        public int idperusahaan { get; set; }

        // [DbColumn("namaperusahaan")]
        public string namaperusahaan { get; set; }

        // [DbColumn("alamat")]
        public string alamat { get; set; }

        // [DbColumn("direktur")]
        public string direktur { get; set; }

        // [DbColumn("kontakdirektur")]
        public string kontakdirektur { get; set; }

        // [DbColumn("emaildirektur")]
        public string emaildirektur { get; set; }

        // [DbColumn("logo")]
        public string logo { get; set; }

        [NotMapped]
        public byte[] DataPhoto { get; set; }

    }
}