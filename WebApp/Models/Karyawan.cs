using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    // [TableName("Karyawan")]
    public class Karyawan
    {
        [Key]
        // [PrimaryKey("idkaryawan")]
        // [DbColumn("idkaryawan")]
        public int idkaryawan { get; set; }

        // [DbColumn("idperusahaan")]

        public int idperusahaan { get; set; }

        // [DbColumn("kodekaryawan")]
        public string kodekaryawan { get; set; }

        // [DbColumn("namakaryawan")]
        public string namakaryawan { get; set; }

        // [DbColumn("jabatan")]
        public string jabatan { get; set; }

        // [DbColumn("alamat")]
        public string alamat { get; set; }

        // [DbColumn("kontak")]
        public string kontak { get; set; }

        // [DbColumn("email")]
        public string email { get; set; }

        // [DbColumn("userid")]

        public string userid { get; set; }

        // [DbColumn("photo")]
        public string photo { get; set; }

        [NotMapped]
        public byte[] DataPhoto { get; set; }

        [ForeignKey("idperusahaan")]
        public Perusahaan perusahaan { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }

        [NotMapped]
        public List<Pelanggaran> Pelanggaran { get; set; }
    }
}