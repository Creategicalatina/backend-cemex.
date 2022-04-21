using AutoMapper;
using back_end_cemex.DTOs;
using back_end_cemex.Entities;

namespace back_end_cemex.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ConveyorDTO, Conveyor>();
            CreateMap<TypeConveyor, TypeConveyorListDTO> ();
            CreateMap<Conveyor, ConveyorListDTO> ();
            CreateMap<Driver, DriverListDTO> ();
        }
    }
}
