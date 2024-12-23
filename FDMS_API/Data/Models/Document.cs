﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal Version {  get; set; }
        public string? Note {  get; set; }
        [Column(TypeName = "varchar(255)")]
        public string FilePath {  get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Foreign property

        public int TypeID { get; set; }
        public int UserID { get; set; }
        public int FlightID { get; set; }

        // Foreign Key Link
        public Type Type { get; set; }
        public User User { get; set; }
        public Flight Flight { get; set; }
        public ICollection<DocumentPermission> DocumentPermissions { get; set; }
        public ICollection<VersionDocument> Versions { get; set; }
    }
}
