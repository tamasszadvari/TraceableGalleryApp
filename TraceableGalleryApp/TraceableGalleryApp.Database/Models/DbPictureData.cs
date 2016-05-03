using SQLite.Net.Attributes;
using TraceableGalleryApp.Interfaces;

namespace TraceableGalleryApp.Database.Models
{
    [Table("UserData")]
    public class DbPictureData : IDbPictureData
    {
        [PrimaryKey, Unique, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("Path")]
        public string Path { get; set; }

        [Column("Labels")]
        public string[] Labels { get; set; }

        [Indexed, Column("XPosition")]
        public double XPosition { get; set; }

        [Indexed, Column("YPosition")]
        public double YPosition { get; set; }
    }
}

