using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_cemex.Entities
{
    public class Conveyor
    {
        [Key]
        [Column("IdConveyor")]
        public int IdConveyor { get; set; }

        [Required(ErrorMessage = "El tipo transportador es requerido")]
        public int TypeConveyorId { get; set; }
        public TypeConveyor TypeConveyor { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
        
    }
}
