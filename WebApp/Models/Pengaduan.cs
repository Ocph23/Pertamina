using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Pengaduan
    {
        [Key]
        public int Id { get; set; }


        public string JenisPengaduan { get; set; }

        public int PengaduanDetailId { get; set; }

    }
}