using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models
{
    [TableName("Karyawan")]
    public class Karyawan
    {

        [PrimaryKey("idkaryawan")]
        [DbColumn("idkaryawan")]
        public int idkaryawan { get; set; }

        [DbColumn("idperusahaan")]
        public int idperusahaan { get; set; }

        [DbColumn("kodekaryawan")]
        public string kodekaryawan { get; set; }

        [DbColumn("namakaryawan")]
        public string namakaryawan { get; set; }

        [DbColumn("jabatan")]
        public string jabatan { get; set; }

        [DbColumn("alamat")]
        public string alamat { get; set; }

        [DbColumn("kontak")]
        public string kontak { get; set; }

        [DbColumn("email")]
        public string email { get; set; }

        [DbColumn("userid")]
        public string userid { get; set; }

        [DbColumn("photo")]
        public string photo { get; set; }
        
        public Perusahaan perusahaan { get; set; }

    }
}