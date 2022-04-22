using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_cemex.Entities
{
    public class Driver
    {
        [Key]
        [Column("IdDriver")]
        public int IdDriver { get; set; }
        public User User { get; set; }
        public int? TypeConveyorId { get; set; }
        public TypeConveyor TypeConveyor { get; set; }
        public int? ConveyorId { get; set; }
        public Conveyor Conveyor { get; set; }

        [Required(ErrorMessage = "El código Sap es requerido")]
        public string CodeSap { get; set; }
        public bool Status { get; set; }
        public string? DocumentDrivinglicenseFrontal { get; set; }
        public string? DocumentDrivinglicenseBack { get; set; }
        public string? DocumentSecurityCard { get; set; }

    }
}
