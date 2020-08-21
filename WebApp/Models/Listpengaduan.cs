using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models
{
    [TableName("Listpengaduan")]
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