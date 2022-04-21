using back_end_cemex.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace back_end_cemex.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController: ControllerBase
    {
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return new List<User>()
            {
                new User() {
                    //UserId = 1,
                    FirstName = "Roro",
                    LastName = "Tombe",
                    Document = "312312",
                    Email = "silviotista93@gmail.com",
                    //Password = "oasmoamsa",
                    //Phone = "312412123",
                    Slug = "roro-tombe",
                    Status = true
                }
            };
        }
    }
}
