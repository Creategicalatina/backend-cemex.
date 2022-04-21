using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class ConveyorManTruckCreationDTO
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
           DATOS PROPIOS DEL TRANSPORTADOR
       =============================================*/
        [Required(ErrorMessage = "El código Sap es requerido")]
        public string CodeSap { get; set; }

        [Required(ErrorMessage = "El tipo transportador es requerido")]
        public int TypeConveyorId { get; set; }
        public IFormFile? DocumentDrivinglicenseFrontal { get; set; }
        public IFormFile? DocumentDrivinglicenseBack { get; set; }
        public IFormFile? DocumentIdentityCardFrontal { get; set; }
        public IFormFile? DocumentIdentityCardBack { get; set; }
    }
}
