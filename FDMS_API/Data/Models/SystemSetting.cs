﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class SystemSetting
    {
        [Key]
        public int ID { get; set; }
        public int Theme { get; set; }
        [Column(TypeName ="varchar(255)")]
        public string LogoURL { get; set; }
        public bool IsCaptchaRequired { get; set; }      
    }
}
