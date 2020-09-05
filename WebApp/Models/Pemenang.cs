using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{

    public class Pemenang
    {
        [Key]
        public int Id { get; set; }

        public int PeriodeId { get; set; }
        public Periode Periode { get; set; }

        public int KaryawanId { get; set; }
        public Karyawan Karyawan{get;set;}

    }
}