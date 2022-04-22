using Microsoft.AspNetCore.Http;

namespace back_end_cemex.DTOs
{
    public class UpdatePhotoSecurityCardDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UrlActualSecurityCard { get; set; }
  
        public IFormFile? DocumentSecurityCard { get; set; }

    }
}
