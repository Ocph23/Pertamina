using System.Linq;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApp.Models
{
    public class Karyawan
    {

        [Key]
        public int Id { get; set; }
        public string KodeKaryawan { get; set; }

        public string NamaKaryawan { get; set; }

        public string Alamat { get; set; }

        public string Kontak { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public string Photo { get; set; }

        public bool Status { get; set; } = true;

        [NotMapped]
        public byte[] DataPhoto { get; set; }

        [NotMapped]
        public PerusahaanKaryawan PerusahaanKaryawan
        {
            get
            {
                if (_perusahaan != null)
                {
                    return _perusahaan;
                }

                if (Perusahaans != null && Perusahaans.Count > 0)
                {
                    return Perusahaans.Last();
                }

                return _perusahaan;
            }
            set { _perusahaan = value; }
        }

        [NotMapped]
        public virtual ICollection<string> Roles { get; set; }

        public virtual ICollection<Pelanggaran> Pelanggarans { get; set; }

        public virtual ICollection<Absen> Absens { get; set; }

        public ICollection<PerusahaanKaryawan> Perusahaans { get; set; } = new List<PerusahaanKaryawan>();

        private PerusahaanKaryawan _perusahaan;
    }
}