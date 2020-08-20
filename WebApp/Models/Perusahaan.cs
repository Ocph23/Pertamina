using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models
{
    [TableName("Perusahaan")]
    public class Perusahaan
    {
        [PrimaryKey("idperusahaan")]
        [DbColumn("idperusahaan")]
        public int idperusahaan { get; set; }

        [DbColumn("namaperusahaan")]
        public string namaperusahaan { get; set; }

        [DbColumn("alamat")]
        public string alamat { get; set; }

        [DbColumn("direktur")]
        public string direktur { get; set; }

        [DbColumn("kontakdirektur")]
        public string kontakdirektur { get; set; }

        [DbColumn("emaildirektur")]
        public string emaildirektur { get; set; }

        [DbColumn("logo")]
        public string logo { get; set; }


        public byte[] DataPhoto { get; set; }

    }
}