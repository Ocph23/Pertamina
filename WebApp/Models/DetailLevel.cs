using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class DetailLevel
    {

        [Key]
        public int Id { get; set; }

        public string Nama { get; set; }

        public double NilaiKaryawan { get; set; }

        public double NilaiPerusahaan { get; set; }

        public double Penambahan { get; set; }

        public int LevelId { get; set; }

        public virtual Level Level { get; set; }


    }
}