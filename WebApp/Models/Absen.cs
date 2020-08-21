using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Absen
    {
        [Key]
        public int idabsen { get; set; }

        [ForeignKey("Karyawan")]
        public int idkaryawan { get; set; }

    }
}