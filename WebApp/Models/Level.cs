using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Level
    {
        [Key]
        public int Id { get; set; }

        public string Nama { get; set; }

        public virtual ICollection<DetailLevel> Datas { get; set; }
    }
}