using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Listpengaduan
    {
        // [PrimaryKey ("idlistpengaduan")]
        // [DbColumn ("idlistpengaduan")]
        public int idlistpengaduan { get; set; }

        // [DbColumn ("penambahanpoint")]
        public double penambahanpoint { get; set; }

        // [DbColumn ("namapengaduan")]
        public string namapengaduan { get; set; }

    }
}