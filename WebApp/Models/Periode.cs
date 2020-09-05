using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Periode
    {
        [Key]
        public int Id { get; set; }

        public DateTime Mulai { get; set; }

        public DateTime Selesai { get; set; }

        public DateTime Undian { get; set; }

        public bool Status { get; set; }

    }
}