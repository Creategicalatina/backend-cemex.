using back_end_cemex.Entities;
using System.ComponentModel.DataAnnotations;

namespace back_end_cemex.DTOs
{
    public class DriverListDTO
    {
        
        public string CodeSap { get; set; }
        public User User { get; set; }
        public int ConveyorId { get; set; }
        public int TypeConveyorId { get; set; }
        public string? DocumentDrivinglicenseFrontal { get; set; }
        public string? DocumentDrivinglicenseBack { get; set; }
   
    }
}
