using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class RequestForgotPassword
    {
        [Required(ErrorMessage = "El corroe electrónico es requerido")]
        [EmailAddress(ErrorMessage = "No es una dirección de correo electrónico válida.")]
        public string Email { get; set; }
       
    }
}
