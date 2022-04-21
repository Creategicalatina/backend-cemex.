using AutoMapper;
using back_end_cemex.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end_cemex.Controllers
{
    [ApiController]
    [Route("api/type-conveyor")]
    public class TypeConveyorController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TypeConveyorController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("list-type-conveyors")]
        public async Task<List<TypeConveyorListDTO>>Get()
        {
            var typeConveyors = await context.TypeConveyors.ToListAsync();
            return mapper.Map<List<TypeConveyorListDTO>>(typeConveyors);
        }
    }
}
