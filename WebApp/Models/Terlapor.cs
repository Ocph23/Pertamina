using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Terlapor
    {
        [Key]
        public int Id { get; set; }

        public int IdPengadian { get; set; }

        public int KaryawanId { get; set; }

        public int JenisPelanggaranId { get; set; }

    }
}