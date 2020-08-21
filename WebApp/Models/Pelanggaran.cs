using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    // [TableName("Pelanggaran")]
    public class Pelanggaran
    {
        [Key]

        // [PrimaryKey("idpelanggaran")]
        // [DbColumn("idpelanggaran")]
        public int idpelanggaran { get; set; }

        // [DbColumn("idjenispelanggaran")]
        public int idjenispelanggaran { get; set; }

        // [DbColumn("idkaryawan")]

        public int idkaryawan { get; set; }

        // [DbColumn("karyawan")]
        public double karyawan { get; set; }

        // [DbColumn("perusahaan")]
        public double perusahaan { get; set; }

        // [DbColumn("tanggal")]
        public DateTime? tanggal { get; set; }


        [ForeignKey("idjenispelanggaran")]
        public Jenispelanggaran Jenispelanggaran { get; set; }

        [ForeignKey("IdPelanggaran")]
        public List<DataFile> Files { get; set; }

        [NotMapped]
        public Level Level { get; set; }

    }
}