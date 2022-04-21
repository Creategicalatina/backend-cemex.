using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_cemex.Entities
{
    public class User : IdentityUser
    {  

        [Required]
        [StringLength(maximumLength:100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 15)]
        public string Document { get; set; }

        /* [Required]
         [EmailAddress]
         [StringLength(maximumLength: 15)]
         public string Email { get; set; }

         [Required]
         public string Password { get; set; } */

        public bool Status { get; set; }

        //public string Phone { get; set; }
        [StringLength(maximumLength: 100)]
        public string Slug { get; set; }
        public string? DocumentIdentityCardFrontal { get; set; }
        public string? DocumentIdentityCardBack { get; set; }

    }
}
