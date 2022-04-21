using Microsoft.AspNetCore.Http;

namespace back_end_cemex.DTOs
{
    public class UpdatePhotoIdentityCardDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string? UrlActualIdentityCardFrontal { get; set; }
        public string? UrlActualIdentityCardBack { get; set; }
        public IFormFile? DocumentIdentityCardFrontal { get; set; }
        public IFormFile? DocumentIdentityCardBack { get; set; }
    }
}
