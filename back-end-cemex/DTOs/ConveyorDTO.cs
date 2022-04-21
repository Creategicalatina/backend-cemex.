using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class ConveyorDTO
    {
        /*=============================================
           DATOS DEL USUARIO
        =============================================*/
        [Required]
        [StringLength(maximumLength: 100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 15)]
        public string Document { get; set; }

        [Required]
        public string Email { get; set; }

        [StringLength(maximumLength: 15)]
        public string PhoneNumber { get; set; }

        [Required]
        public string role { get; set; }

        /*=============================================
           DATOS PROPIOS PARA SOLICITAR EMPRESA
        =============================================*/
      
        public string NameCompany { get; set; }
  
        public string NitCompany { get; set; }
        public string DocumentCompany { get; set; }

        /*=============================================
            DATOS PROPIOS DEL TRANSPORTADOR
        =============================================*/
        [Required(ErrorMessage = "El código Sap es requerido")]
        public string CodeSap { get; set; }

        [Required(ErrorMessage = "El tipo transportador es requerido")]
        public int TypeConveyorId { get; set; }

    }
}
