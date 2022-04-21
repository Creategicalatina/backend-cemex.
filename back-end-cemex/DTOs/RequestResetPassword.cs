using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class RequestResetPassword
    {
        [Required(ErrorMessage = "Contraseña es requerida.")]
        [DataType(DataType.Password, ErrorMessage = "La contraseña no es válida.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña de confirmación no coincide.")]
        [DataType(DataType.Password, ErrorMessage = "Las contraseñas deben tener al menos una mayúscula ('A'-'Z').")]
   
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        
    }
}
