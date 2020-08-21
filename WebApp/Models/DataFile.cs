using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{

    // [TableName("buktipelanggaran")]
    public class DataFile
    {
        // [PrimaryKey("Id")]
        // [DbColumn("Id")]
        [Key]
        public int Id { get; set; }

        // [DbColumn("idpelanggaran")]
        public int IdPelanggaran { get; set; }

        // [DbColumn("filetype")]
        public string FileType { get; set; }

        // [DbColumn("filename")]
        public string FileName { get; set; }


        // [DbColumn("thumb")]
        public string Thumb { get; set; }

        [NotMapped]

        public byte[] Data { get; set; }


    }
}