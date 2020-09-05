using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{

    public class BuktiPelanggaran
    {
        [Key]
        public int Id { get; set; }

        public int PelanggaranId { get; set; }

        public string FileType { get; set; }

        public string FileName { get; set; }

        public string Thumb { get; set; }


        [NotMapped]

        public byte[] Data { get; set; }


    }
}