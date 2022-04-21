using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class RequestLogin
    {
        [Required]
        [EmailAddress(ErrorMessage = "No es una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
