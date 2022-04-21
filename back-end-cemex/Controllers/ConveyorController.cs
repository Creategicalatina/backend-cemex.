using AutoMapper;
using back_end_cemex.DTOs;
using back_end_cemex.Entities;
using back_end_cemex.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace back_end_cemex.Controllers
{
    [ApiController]
    [Route("api/conveyor")]
    public class ConveyorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IStorageArchives storageArchives;
        private readonly string contenedor = "documents";

        public ConveyorController(ApplicationDbContext context, IMapper mapper, 
            UserManager<User> userManager, IStorageArchives storageArchives
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.storageArchives = storageArchives;
        }

        [HttpGet("list-conveyors")]
        public async Task<List<ConveyorListDTO>> Get()
        {
            var conveyors = await context.Conveyors.
                Include(with => with.User).Include(with => with.Company).ToListAsync();
            return mapper.Map<List<ConveyorListDTO>>(conveyors);
        }

        [HttpPost("register-admin-logist-third")]
        public async Task<ActionResult> CreationAdminLogistThird([FromForm]ConveyorAdminLogistThirdCreationDTO conveyorAdminLogist)
        {
            var randomString = RandomString(10);
       
            var urlDocumentCompany = "";

            var urlDocumentIndetityCardFrontal = "";
            var urlDocumentIndetityCardBack = "";

            /*=============================================
              GUARDAMOS EL DOCUMENTO DE LA EMPRESA
             =============================================*/
            if (conveyorAdminLogist.DocumentCompany != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorAdminLogist.DocumentCompany.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorAdminLogist.DocumentCompany.FileName);
                    urlDocumentCompany = await storageArchives.SaveArchive(contenido, $"document-company-{conveyorAdminLogist.FirstName}-{conveyorAdminLogist.LastName}-{randomString}", extension,
                        contenedor, conveyorAdminLogist.DocumentCompany.ContentType);
                }
            }
            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA FRONTAL
             =============================================*/
            if (conveyorAdminLogist.DocumentIdentityCardFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorAdminLogist.DocumentIdentityCardFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorAdminLogist.DocumentIdentityCardFrontal.FileName);
                    urlDocumentIndetityCardFrontal = await storageArchives.SaveArchive(contenido, $"identity-card-frontal-{conveyorAdminLogist.FirstName}-{conveyorAdminLogist.LastName}-{randomString}", extension,
                        contenedor, conveyorAdminLogist.DocumentIdentityCardFrontal.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA POSTERIOR
             =============================================*/
            if (conveyorAdminLogist.DocumentIdentityCardBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorAdminLogist.DocumentIdentityCardBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorAdminLogist.DocumentIdentityCardBack.FileName);
                    urlDocumentIndetityCardBack = await storageArchives.SaveArchive(contenido, $"identity-card-back-{conveyorAdminLogist.FirstName}-{conveyorAdminLogist.LastName}-{randomString}", extension,
                        contenedor, conveyorAdminLogist.DocumentIdentityCardBack.ContentType);
                }
            }

            using var transaction = context.Database.BeginTransaction(); //Inicializamos la transacción
            try
            {
                var existEmailUser = await context.Users.AnyAsync(x => x.Email == conveyorAdminLogist.Email);

                if (existEmailUser)
                {
                    return BadRequest($"El correo electrónico {conveyorAdminLogist.Email} ya se encuentra registrado");
                }
                var existDocumentUser = await context.Users.AnyAsync(x => x.Document == conveyorAdminLogist.Document);

                if (existDocumentUser)
                {
                    return BadRequest($"El número de documento {conveyorAdminLogist.Document} ya se encuentra registrado");
                }
                // Instaciamos un nuevo usuario para crearlo
                var name = conveyorAdminLogist.FirstName.ToLower();
                var lastName = conveyorAdminLogist.LastName.ToLower();
                var password = "Cemex.2310";
                var user = new User
                {
                    UserName = conveyorAdminLogist.Email,
                    FirstName = char.ToUpper(name[0]) + name.Substring(1),
                    LastName = char.ToUpper(lastName[0]) + lastName.Substring(1),
                    Document = conveyorAdminLogist.Document,
                    Email = conveyorAdminLogist.Email,
                    PhoneNumber = conveyorAdminLogist.PhoneNumber,
                    Status = false,
                    Slug = conveyorAdminLogist.FirstName,
                    DocumentIdentityCardFrontal = urlDocumentIndetityCardFrontal,
                    DocumentIdentityCardBack = urlDocumentIndetityCardBack,

                };
                var company = new Company();

                //Creamos el usuario
                var result = await userManager.CreateAsync(user, password);
                //Si al crear el usuario todo sale bien
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, conveyorAdminLogist.role); //Asingamos el rol

                    var existCompany = await context.Companies.AnyAsync(x => x.NitCompany == conveyorAdminLogist.NitCompany);

                    if (existCompany)
                    {
                        return BadRequest($"El número de Nit {conveyorAdminLogist.NitCompany} ya se encuentra registrado");
                    }
                     company = new Company
                    {
                        NameCompany = conveyorAdminLogist.NameCompany,
                        NitCompany = conveyorAdminLogist.NitCompany,
                        DocumentCompany = urlDocumentCompany
                     };
                    var response = context.Add(company);
                    await context.SaveChangesAsync();

                    // Instaciamos un nuevo transportador para crearlo
                    var conveyor = new Conveyor
                    {
                        TypeConveyorId = conveyorAdminLogist.TypeConveyorId,
                        User = user,
                        Company = company,
                    };

                    context.Add(conveyor);
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

        [HttpPost("register-mantruck")]
        public async Task<ActionResult> CreationManTruck([FromForm]ConveyorManTruckCreationDTO conveyorManTruck)
        {
            var randomString = RandomString(10);
            var urlDocumentDrivingLinceseFrontal = "";
            var urlDocumentDrivingLinceseBack = "";

            var urlDocumentIndetityCardFrontal = "";
            var urlDocumentIndetityCardBack = "";

            /*=============================================
              GUARDAMOS EL DOCUMENTO DE LICENCIA FRONTAL
             =============================================*/
            if (conveyorManTruck.DocumentDrivinglicenseFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorManTruck.DocumentDrivinglicenseFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorManTruck.DocumentDrivinglicenseFrontal.FileName);
                    urlDocumentDrivingLinceseFrontal = await storageArchives.SaveArchive(contenido, $"licence-frontal-{conveyorManTruck.FirstName}-{conveyorManTruck.LastName}-{randomString}", extension,
                        contenedor, conveyorManTruck.DocumentDrivinglicenseFrontal.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO DE LICENCIA POSTERIOR
             =============================================*/
            if (conveyorManTruck.DocumentDrivinglicenseBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorManTruck.DocumentDrivinglicenseBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorManTruck.DocumentDrivinglicenseBack.FileName);
                    urlDocumentDrivingLinceseBack = await storageArchives.SaveArchive(contenido, $"licence-back-{conveyorManTruck.FirstName}-{conveyorManTruck.LastName}-{randomString}", extension,
                        contenedor, conveyorManTruck.DocumentDrivinglicenseBack.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA FRONTAL
             =============================================*/
            if (conveyorManTruck.DocumentIdentityCardFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorManTruck.DocumentIdentityCardFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorManTruck.DocumentIdentityCardFrontal.FileName);
                    urlDocumentIndetityCardFrontal = await storageArchives.SaveArchive(contenido, $"identity-card-frontal-{conveyorManTruck.FirstName}-{conveyorManTruck.LastName}-{randomString}", extension,
                        contenedor, conveyorManTruck.DocumentIdentityCardFrontal.ContentType);
                }
            }

            /*=============================================
              GUARDAMOS EL DOCUMENTO CÉDULA POSTERIOR
             =============================================*/
            if (conveyorManTruck.DocumentIdentityCardBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await conveyorManTruck.DocumentIdentityCardBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(conveyorManTruck.DocumentIdentityCardBack.FileName);
                    urlDocumentIndetityCardBack = await storageArchives.SaveArchive(contenido, $"identity-card-back-{conveyorManTruck.FirstName}-{conveyorManTruck.LastName}-{randomString}", extension,
                        contenedor, conveyorManTruck.DocumentIdentityCardBack.ContentType);
                }
            }

            using var transaction = context.Database.BeginTransaction(); //Inicializamos la transacción
            try {
                var existEmailUser = await context.Users.AnyAsync(x => x.Email == conveyorManTruck.Email);

                if (existEmailUser)
                {
                    return BadRequest($"El correo electrónico {conveyorManTruck.Email} ya se encuentra registrado");
                }
                var existDocumentUser = await context.Users.AnyAsync(x => x.Document == conveyorManTruck.Document);

                if (existDocumentUser)
                {
                    return BadRequest($"El número de documento {conveyorManTruck.Document} ya se encuentra registrado");
                }
                // Instaciamos un nuevo usuario para crearlo
                var name = conveyorManTruck.FirstName.ToLower();
                var lastName = conveyorManTruck.LastName.ToLower();
                var password = "Cemex.2310";
                var user = new User
                {
                    UserName = conveyorManTruck.Email,
                    FirstName = char.ToUpper(name[0]) + name.Substring(1),
                    LastName = char.ToUpper(lastName[0]) + lastName.Substring(1),
                    Document = conveyorManTruck.Document,
                    Email = conveyorManTruck.Email,
                    PhoneNumber = conveyorManTruck.PhoneNumber,
                    Status = false,
                    Slug = conveyorManTruck.FirstName,
                    DocumentIdentityCardFrontal = urlDocumentIndetityCardFrontal,
                    DocumentIdentityCardBack = urlDocumentIndetityCardBack,

                };
                
                //Creamos el usuario
                var result = await userManager.CreateAsync(user, password);
                //Si al crear el usuario todo sale bien
                if (result.Succeeded)
                {
                    //Verificamos el rol seleccionado que viene de la api y lo asignamos al usuario
                    if (conveyorManTruck.role == "ManTruck")
                    {
                        await userManager.AddToRoleAsync(user, conveyorManTruck.role);
                        await userManager.AddToRoleAsync(user, "Driver");

                    }

                    var existCodeSAP = await context.Drivers.AnyAsync(x => x.CodeSap == conveyorManTruck.CodeSap);

                    if (existCodeSAP)
                    {
                        return BadRequest($"El código SAP {conveyorManTruck.CodeSap} ya se encuentra registrado");
                    }

                    // Instaciamos un nuevo transportador para crearlo
                    var conveyor = new Conveyor
                    {
                        TypeConveyorId = conveyorManTruck.TypeConveyorId,
                        User = user,
                    };

                    context.Add(conveyor);
                    await context.SaveChangesAsync();

                    var driver = new Driver
                    {
                        CodeSap = conveyorManTruck.CodeSap,
                        User = user,
                        TypeConveyorId = conveyorManTruck.TypeConveyorId,
                        ConveyorId = conveyor.IdConveyor,
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

        private async Task<ActionResult> SaveCompany(ConveyorDTO conveyorDTO)
        {
            var existCompany = await context.Companies.AnyAsync(x => x.NitCompany == conveyorDTO.NitCompany);

            if (existCompany)
            {
                return BadRequest($"El número de Nit {conveyorDTO.NitCompany} ya se encuentra registrado");
            }
            var company = new Company
            {
                NameCompany = conveyorDTO.NameCompany,
                NitCompany = conveyorDTO.NitCompany,
                DocumentCompany = conveyorDTO.DocumentCompany,
            };
            var response = context.Add(company);
            await context.SaveChangesAsync();
           

            return Ok();
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
