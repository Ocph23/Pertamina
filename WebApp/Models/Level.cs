using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Level
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<JenisPelanggaran> Datas { get; set; }
    }
}