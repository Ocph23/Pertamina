using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;

namespace WebApp.Models
{

    [TableName("buktipelanggaran")]
    public class DataFile
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id { get; set; }

        [DbColumn("idpelanggaran")]
        public int IdPelanggaran { get; set; }

        [DbColumn("filetype")]
        public string FileType { get; set; }

        [DbColumn("filename")]
        public string FileName { get; set; }


        [DbColumn("thumb")]
        public string Thumb { get; set; }

        public byte[] Data { get; set; }


    }
}