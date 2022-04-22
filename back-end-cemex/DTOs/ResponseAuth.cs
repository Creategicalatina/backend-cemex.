using back_end_cemex.Entities;
using System;
using System.Collections.Generic;

namespace back_end_cemex.DTOs
{
    public class ResponseAuth
    {
        public User User { get; set; }
        public List<string> Roles { get; set; }

        public int? IdDriver { get; set; }
        public string? CodeSap { get; set; }
        public bool? Status { get; set; }
        public string? DocumentDrivinglicenseFrontal { get; set; }
        public string? DocumentDrivinglicenseBack { get; set; }
        public string? DocumentSecurityCard { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? DocumentCompany { get; set; }

        public string? Token { get; set; }  
        public DateTime? Expiracion { get; set; }  
        public bool? IsAuthenticated { get; set; }
        public string? Error { get; set; }
        
    }
}
