using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_cemex.Entities
{
    public class Company
    {
        [Key]
        [Column("IdCompany")]
        public int IdCompany { get; set; }
        [Required]
        public string NameCompany { get; set; }

        [Required]
        public string NitCompany { get; set; }
        public string? DocumentCompany { get; set; }

    }
}
