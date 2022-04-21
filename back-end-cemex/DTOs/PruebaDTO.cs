using back_end_cemex.Entities;
using System.Collections.Generic;

namespace back_end_cemex.DTOs
{
    public class PruebaDTO
    {
        public User User { get; set; }
        public List<string> Roles { get; set; }

        public int IdDriver { get; set; }
        public string? CodeSap { get; set; }
        public bool? status { get; set; }
        public string? DocumentDrivinglicenseFrontal { get; set; }
        public string? DocumentDrivinglicenseBack { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
