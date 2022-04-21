using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class CredentialsUser
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string LastNanme { get; set; }

        [Required]
        [StringLength(maximumLength: 15)]
        public string Document { get; set; }

         [Required]
         public string Email { get; set; }

         [Required]
         public string Password { get; set; } 

        public bool Status { get; set; }

        [StringLength(maximumLength: 15)]
        public string PhoneNumber { get; set; }

        [Required]
        public string role { get; set; } 



    }
}
