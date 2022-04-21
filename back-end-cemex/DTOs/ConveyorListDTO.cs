using back_end_cemex.Entities;

namespace back_end_cemex.DTOs
{
    public class ConveyorListDTO
    {
        public int IdConveyor { get; set; }
        public string CodeSap { get; set; }
        public int TypeConveyorId { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
    }
}
