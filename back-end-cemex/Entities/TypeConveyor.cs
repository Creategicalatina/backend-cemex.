using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_cemex.Entities
{
    public class TypeConveyor
    {
        [Key]
        [Column("IdTypeConveyor")]
        public int IdTypeConveyor { get; set; }
        [Required]
        public string? NameTypeConveyor { get; set; } 
        [Required]
        public string? DescriptionTypeConveyor { get; set; } 
    }
}
