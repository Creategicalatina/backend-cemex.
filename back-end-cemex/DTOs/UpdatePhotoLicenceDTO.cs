using Microsoft.AspNetCore.Http;

namespace back_end_cemex.DTOs
{
    public class UpdatePhotoLicenceDTO
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string? UrlActualLicenceFrontal { get; set; }
        public string? UrlActualLicenceBack { get; set; }
        public IFormFile? DocumentDrivinglicenseFrontal { get; set; }
        public IFormFile? DocumentDrivinglicenseBack { get; set; }
    }
}