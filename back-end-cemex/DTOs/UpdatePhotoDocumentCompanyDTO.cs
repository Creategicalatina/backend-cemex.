using Microsoft.AspNetCore.Http;

namespace back_end_cemex.DTOs
{
    public class UpdatePhotoDocumentCompanyDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UrlActualDocumentCompany { get; set; }
        public IFormFile? DocumentCompany { get; set; }
    }
}
