using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Level
    {
        [Key]
        public int idlevel { get; set; }

        public string level { get; set; }

        [ForeignKey("idlevel")]
        public ICollection<Jenispelanggaran> Datas { get; set; }

    }
}