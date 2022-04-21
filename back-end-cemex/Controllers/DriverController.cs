using AutoMapper;
using back_end_cemex.DTOs;
using back_end_cemex.Entities;
using back_end_cemex.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace back_end_cemex.Controllers
{
    [ApiController]
    [Route("api/driver")]
    public class DriverController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IStorageArchives storageArchives;
        private readonly string contenedor = "documents";

        public DriverController(ApplicationDbContext context, IMapper mapper, 
            UserManager<User> userManager, IStorageArchives storageArchives)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.storageArchives = storageArchives;
        }

        [HttpGet("{IDriver:int}", Name = "list-driver-for-id")]
        public async Task<ActionResult<DriverListDTO>> Get(int IDriver)
        {
            var driver = await context.Drivers.
                FirstOrDefaultAsync(driver => driver.IdDriver == IDriver);
            
            return mapper.Map<DriverListDTO>(driver);
        }

        /*[HttpGet("{User:string}", Name = "list-driver-for-id-user")]
        public async Task<ActionResult<DriverListDTO>>GetDriverForUser(string User)
        {
            var driver = await context.Drivers.
                FirstOrDefaultAsync(driver => driver.User == User);

            return mapper.Map<DriverListDTO>(driver);
        }*/

        [HttpPost("register")]
        public async Task<ActionResult>CreationDriver([FromForm]DriverCreationDTO driverCreationDTO)
        {
            var randomString = RandomString(10);
            var urlDocumentDrivingLinceseFrontal = "";
            var urlDocumentDrivingLinceseBack = "";

            var urlDocumentIndetityCardFrontal = "";
            var urlDocumentIndetityCardBack = "";

            /*=============================================
              GUARDAMOS EL DOCUMENTO DE LICENCIA FRONTAL
             =============================================*/
            if (driverCreationDTO.DocumentDrivinglicenseFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await driverCreationDTO.DocumentDrivinglicenseFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(driverCreationDTO.DocumentDrivinglicenseFrontal.FileName);
                    urlDocumentDrivingLinceseFrontal = await storageArchives.SaveArchive(contenido, $"licence-frontal-{driverCreationDTO.FirstName}-{driverCreationDTO.LastName}-{randomString}",extension,
                        contenedor, driverCreationDTO.DocumentDrivinglicenseFrontal.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO DE LICENCIA POSTERIOR
             =============================================*/
            if (driverCreationDTO.DocumentDrivinglicenseBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await driverCreationDTO.DocumentDrivinglicenseBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(driverCreationDTO.DocumentDrivinglicenseBack.FileName);
                    urlDocumentDrivingLinceseBack = await storageArchives.SaveArchive(contenido, $"licence-back-{driverCreationDTO.FirstName}-{driverCreationDTO.LastName}-{randomString}", extension,
                        contenedor, driverCreationDTO.DocumentDrivinglicenseBack.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA FRONTAL
             =============================================*/
            if (driverCreationDTO.DocumentIdentityCardFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await driverCreationDTO.DocumentIdentityCardFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(driverCreationDTO.DocumentIdentityCardFrontal.FileName);
                    urlDocumentIndetityCardFrontal = await storageArchives.SaveArchive(contenido, $"identity-card-frontal-{driverCreationDTO.FirstName}-{driverCreationDTO.LastName}-{randomString}", extension,
                        contenedor, driverCreationDTO.DocumentIdentityCardFrontal.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA POSTERIOR
             =============================================*/
            if (driverCreationDTO.DocumentIdentityCardBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await driverCreationDTO.DocumentIdentityCardBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(driverCreationDTO.DocumentIdentityCardBack.FileName);
                    urlDocumentIndetityCardBack = await storageArchives.SaveArchive(contenido, $"identity-card-back-{driverCreationDTO.FirstName}-{driverCreationDTO.LastName}-{randomString}", extension,
                        contenedor, driverCreationDTO.DocumentIdentityCardBack.ContentType);
                }
            }

            using var transaction = context.Database.BeginTransaction(); //Inicializamos la transacción
            try
            {
                var existEmailUser = await context.Users.AnyAsync(x => x.Email == driverCreationDTO.Email);

            if (existEmailUser)
            {
                return BadRequest($"El correo electrónico {driverCreationDTO.Email} ya se encuentra registrado");
            }
            var existDocumentUser = await context.Users.AnyAsync(x => x.Document == driverCreationDTO.Document);

            if (existDocumentUser)
            {
                return BadRequest($"El número de documento {driverCreationDTO.Document} ya se encuentra registrado");
            }
            // Instaciamos un nuevo usuario para crearlo
            var password = "Cemex.2310";
            var name = driverCreationDTO.FirstName.ToLower();
            var lastName = driverCreationDTO.LastName.ToLower();
            var user = new User
            {
                UserName = driverCreationDTO.Email,
                FirstName = char.ToUpper(name[0]) + name.Substring(1),
                LastName = char.ToUpper(lastName[0]) + lastName.Substring(1),
                Document = driverCreationDTO.Document,
                Email = driverCreationDTO.Email,
                PhoneNumber = driverCreationDTO.PhoneNumber,
                Status = false,
                Slug = $"{driverCreationDTO.FirstName}-{driverCreationDTO.LastName}-{randomString}",
                DocumentIdentityCardFrontal = urlDocumentIndetityCardFrontal,
                DocumentIdentityCardBack = urlDocumentIndetityCardBack,

            };
            //Creamos el usuario
            var result = await userManager.CreateAsync(user, password);
            //Si al crear el usuario todo sale bien
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, driverCreationDTO.role);

                var driver = new Driver
                {
                    CodeSap = driverCreationDTO.CodeSap,
                    User = user,
                    TypeConveyorId = driverCreationDTO.TypeConveyorId,
                    ConveyorId = driverCreationDTO.ConveyorId,
                    Status = false,
                    DocumentDrivinglicenseFrontal = urlDocumentDrivingLinceseFrontal,
                    DocumentDrivinglicenseBack = urlDocumentDrivingLinceseBack,
                };
                context.Add(driver);
                await context.SaveChangesAsync();
                transaction.Commit();
                return Ok();

            }
            else
            {
                return BadRequest(result.Errors);
            }
            }
            catch (DbUpdateException exception) when (exception?.InnerException?.Message.Contains("Cannot insert duplicate key row in object") ?? false)
            {

                transaction.Rollback();
                Console.WriteLine("Error al realizar la transacción");
                return BadRequest(exception.InnerException.Message);
            }
        }
        private static string RandomString(int length)
        {
            Random random = new Random();
            const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
