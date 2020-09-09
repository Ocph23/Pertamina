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
        public PerusahaanKaryawan Perusahaan
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
        public ICollection<Pelanggaran> Pelanggarans { get; set; }
        [NotMapped]
        public ICollection<Absen> Absens { get; set; }
        public virtual ICollection<PerusahaanKaryawan> Perusahaans { get; set; } = new List<PerusahaanKaryawan>();

        [NotMapped]
        public List<string> Roles { get; set; }

        private PerusahaanKaryawan _perusahaan;
    }
}