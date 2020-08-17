using System;
using System.Collections.Generic;
using Ocph.DAL;

namespace WebApp.Models
{
    [TableName("Pelanggaran")]
    public class Pelanggaran
    {
        [PrimaryKey("idpelanggaran")]
        [DbColumn("idpelanggaran")]
        public int idpelanggaran { get; set; }

        [DbColumn("idjenispelanggaran")]
        public int idjenispelanggaran { get; set; }

        [DbColumn("idkaryawan")]
        public int idkaryawan { get; set; }

        [DbColumn("karyawan")]
        public double karyawan { get; set; }

        [DbColumn("perusahaan")]
        public int perusahaan { get; set; }

        [DbColumn("tanggal")]
        public DateTime? tanggal { get; set; }

        public Level Level { get; set; }
        public Jenispelanggaran Jenispelanggaran { get; set; }

        public List<DataFile> Files { get; set; }

    }
}