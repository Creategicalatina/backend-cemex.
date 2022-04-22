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
    [Route("api/profile")]
    public class ProfileController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IStorageArchives storageArchives;
        private readonly string contenedor = "documents";

        public ProfileController(ApplicationDbContext context, UserManager<User> userManager,
            IStorageArchives storageArchives
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.storageArchives = storageArchives;
        }

        /*=============================================
            DATOS DEL USUARIO
        =============================================*/
        [HttpGet("get-data-user/{email}", Name = "get-data-user")]
        public async Task<ActionResult<ResponseAuth>> Get(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var roles = await userManager.GetRolesAsync(user);
            var driver = await context.Drivers.FirstOrDefaultAsync(x => x.User == user);
            var conveyor = await context.Conveyors.Include(with => with.TypeConveyor).Include(with => with.Company).FirstOrDefaultAsync(x => x.User == user);

            if (roles[0] == "AdminLogis")
            {
                return new ResponseAuth()
                {
                    User = user,
                    Roles = (List<string>)roles,
                    CompanyId = conveyor.Company.IdCompany,
                    CompanyName = conveyor.Company.NameCompany,
                    DocumentCompany = conveyor.Company.DocumentCompany
                };
            }
            else
            {
                return new ResponseAuth()
                {
                    User = user,
                    Roles = (List<string>)roles,
                    IdDriver = driver.IdDriver,
                    CodeSap = driver.CodeSap,
                    Status = driver.Status,
                    DocumentDrivinglicenseFrontal = driver.DocumentDrivinglicenseFrontal,
                    DocumentDrivinglicenseBack = driver.DocumentDrivinglicenseBack,
                    DocumentSecurityCard = driver.DocumentSecurityCard,
                };
            }

            /*var user = await context.Drivers.
                FirstOrDefaultAsync(driver => driver.IdDriver == IDriver);

            return mapper.Map<DriverListDTO>(driver);*/
        }
        /*=============================================
            ACTUALIZAR CARNE DE SEGURIDAD
        =============================================*/
        [HttpPut("update-photo-security-card/{id:int}")]
        public async Task<ActionResult> UpdatePhotoSecurityCard([FromForm] UpdatePhotoSecurityCardDTO updatePhotoSecurityCardDTO, int id)
        {
            var exist = await context.Drivers.AnyAsync(driver => driver.IdDriver == id);
            var randomString = RandomString(10);
            if (!exist)
            {
                return NotFound("No existe conductor con el id ingresado");
            }

            var urlDocumentSecurityCard = "";
     

            /*=============================================
              SI EL USUARIO NO TIENE CARNE DE SEGURIDAD
             =============================================*/
            if (updatePhotoSecurityCardDTO.UrlActualSecurityCard == null)
            {
                if (updatePhotoSecurityCardDTO.DocumentSecurityCard != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoSecurityCardDTO.DocumentSecurityCard.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoSecurityCardDTO.DocumentSecurityCard.FileName);
                        urlDocumentSecurityCard = await storageArchives.SaveArchive(contenido, $"security-card-{updatePhotoSecurityCardDTO.FirstName}-{updatePhotoSecurityCardDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoSecurityCardDTO.DocumentSecurityCard.ContentType);
                    }
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LICENCIA FRONTAL
             =============================================*/
            if (updatePhotoSecurityCardDTO.DocumentSecurityCard != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoSecurityCardDTO.DocumentSecurityCard.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoSecurityCardDTO.DocumentSecurityCard.FileName);
                    urlDocumentSecurityCard = await storageArchives.EditArchive(contenido, $"security-card-{updatePhotoSecurityCardDTO.FirstName}-{updatePhotoSecurityCardDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoSecurityCardDTO.UrlActualSecurityCard, updatePhotoSecurityCardDTO.DocumentSecurityCard.ContentType);
                }
            }

  

            var result = await context.Drivers.SingleOrDefaultAsync(driver => driver.IdDriver == id);
            if (result != null)
            {
                result.DocumentSecurityCard = urlDocumentSecurityCard;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /*=============================================
            ACTUALIZAR LICENCIA DE CONDUCCIÓN
        =============================================*/
        [HttpPut("{id:int}")] 
        public async Task<ActionResult> UpdatePhotoLicence([FromForm]UpdatePhotoLicenceDTO updatePhotoLicenceDTO, int id)
        {
            var exist = await context.Drivers.AnyAsync(driver => driver.IdDriver == id);
            var randomString = RandomString(10);
            if (!exist)
            {
                return NotFound("No existe conductor con el id ingresado");
            }

            var urlDocumentDrivingLinceseFrontal = "";
            var urlDocumentDrivingLinceseBack = "";

            /*=============================================
              SI EL USUARIO NO TIENE FOTO DE LICENCIA
             =============================================*/
            if (updatePhotoLicenceDTO.UrlActualLicenceFrontal == null)
            {
                if (updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.FileName);
                        urlDocumentDrivingLinceseFrontal = await storageArchives.SaveArchive(contenido, $"licence-frontal-{updatePhotoLicenceDTO.FirstName}-{updatePhotoLicenceDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.ContentType);
                    }
                }

                /*=============================================
                  GUARDAMOS EL DOCUMENTO DE LICENCIA POSTERIOR
                 =============================================*/
                if (updatePhotoLicenceDTO.DocumentDrivinglicenseBack != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoLicenceDTO.DocumentDrivinglicenseBack.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoLicenceDTO.DocumentDrivinglicenseBack.FileName);
                        urlDocumentDrivingLinceseBack = await storageArchives.SaveArchive(contenido, $"licence-back-{updatePhotoLicenceDTO.FirstName}-{updatePhotoLicenceDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoLicenceDTO.DocumentDrivinglicenseBack.ContentType);
                    }
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LICENCIA FRONTAL
             =============================================*/
            if (updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.FileName);
                    urlDocumentDrivingLinceseFrontal = await storageArchives.EditArchive(contenido, $"licence-frontal-{updatePhotoLicenceDTO.FirstName}-{updatePhotoLicenceDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoLicenceDTO.UrlActualLicenceFrontal, updatePhotoLicenceDTO.DocumentDrivinglicenseFrontal.ContentType);
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LICENCIA TRASERO
             =============================================*/
            if (updatePhotoLicenceDTO.DocumentDrivinglicenseBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoLicenceDTO.DocumentDrivinglicenseBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoLicenceDTO.DocumentDrivinglicenseBack.FileName);
                    urlDocumentDrivingLinceseBack = await storageArchives.EditArchive(contenido, $"licence-back-{updatePhotoLicenceDTO.FirstName}-{updatePhotoLicenceDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoLicenceDTO.UrlActualLicenceBack, updatePhotoLicenceDTO.DocumentDrivinglicenseBack.ContentType);
                }
            }

            var result = await context.Drivers.SingleOrDefaultAsync(driver => driver.IdDriver == id);
            if (result != null)
            {
                result.DocumentDrivinglicenseFrontal = urlDocumentDrivingLinceseFrontal;
                result.DocumentDrivinglicenseBack = urlDocumentDrivingLinceseBack;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        /*=============================================
            ACTUALIZAR CEDULA DE CIUDADANIA
        =============================================*/
        [HttpPut("update-photo-identity-card/{email}")]
        public async Task<ActionResult> UpdatePhotoIdentityCard([FromForm] UpdatePhotoIdentityCardDTO updatePhotoIdentityCardDTO, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var randomString = RandomString(10);
            if (user == null)
            {
                return NotFound("No existe el usuario");
            }

            var urlDocumentIdentityCardFrontal = "";
            var urlDocumentIdentityCardBack = "";

            /*=============================================
              SI EL USUARIO NO TIENE FOTO DE LICENCIA
             =============================================*/
            if (updatePhotoIdentityCardDTO.UrlActualIdentityCardFrontal == null)
            {
                if (updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.FileName);
                        urlDocumentIdentityCardFrontal = await storageArchives.SaveArchive(contenido, $"licence-frontal-{updatePhotoIdentityCardDTO.FirstName}-{updatePhotoIdentityCardDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.ContentType);
                    }
                }

                /*=============================================
                  GUARDAMOS EL DOCUMENTO DE LICENCIA POSTERIOR
                 =============================================*/
                if (updatePhotoIdentityCardDTO.DocumentIdentityCardBack != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoIdentityCardDTO.DocumentIdentityCardBack.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoIdentityCardDTO.DocumentIdentityCardBack.FileName);
                        urlDocumentIdentityCardBack = await storageArchives.SaveArchive(contenido, $"licence-back-{updatePhotoIdentityCardDTO.FirstName}-{updatePhotoIdentityCardDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoIdentityCardDTO.DocumentIdentityCardBack.ContentType);
                    }
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LICENCIA FRONTAL
             =============================================*/
            if (updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.FileName);
                    urlDocumentIdentityCardFrontal = await storageArchives.EditArchive(contenido, $"licence-frontal-{updatePhotoIdentityCardDTO.FirstName}-{updatePhotoIdentityCardDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoIdentityCardDTO.UrlActualIdentityCardFrontal, updatePhotoIdentityCardDTO.DocumentIdentityCardFrontal.ContentType);
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LICENCIA TRASERO
             =============================================*/
            if (updatePhotoIdentityCardDTO.DocumentIdentityCardBack != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoIdentityCardDTO.DocumentIdentityCardBack.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoIdentityCardDTO.DocumentIdentityCardBack.FileName);
                    urlDocumentIdentityCardBack = await storageArchives.EditArchive(contenido, $"licence-back-{updatePhotoIdentityCardDTO.FirstName}-{updatePhotoIdentityCardDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoIdentityCardDTO.UrlActualIdentityCardBack, updatePhotoIdentityCardDTO.DocumentIdentityCardBack.ContentType);
                }
            }

            var result = await context.Users.SingleOrDefaultAsync(user => user.Email == email);
            if (result != null)
            {
                result.DocumentIdentityCardFrontal = urlDocumentIdentityCardFrontal;
                result.DocumentIdentityCardBack = urlDocumentIdentityCardBack;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        /*=============================================
            ACTUALIZAR DOCUMENTO DE LA EMPRESA
        =============================================*/
        [HttpPut("update-photo-document-company/{id:int}")]
        public async Task<ActionResult> UpdatePhotoDocumentCompany([FromForm] UpdatePhotoDocumentCompanyDTO updatePhotoDocumentCompanyDTO, int id)
        {
            var exist = await context.Companies.AnyAsync(company => company.IdCompany == id);
            var randomString = RandomString(10);
            if (exist == null)
            {
                return NotFound("No existe la empresa");
            }

            var urlDocumentCompanyFrontal = "";
          

            /*=============================================
              SI EL USUARIO NO TIENE FOTO DOCUMENTO EMPRESA
             =============================================*/
            if (updatePhotoDocumentCompanyDTO.UrlActualDocumentCompany == null)
            {
                /*=============================================
                    GUARDAMOS EL DOCUMENTO DE LA EMPRESA
                =============================================*/
                if (updatePhotoDocumentCompanyDTO.DocumentCompany != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await updatePhotoDocumentCompanyDTO.DocumentCompany.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(updatePhotoDocumentCompanyDTO.DocumentCompany.FileName);
                        urlDocumentCompanyFrontal = await storageArchives.SaveArchive(contenido, $"document-company-{updatePhotoDocumentCompanyDTO.FirstName}-{updatePhotoDocumentCompanyDTO.LastName}-{randomString}", extension,
                            contenedor, updatePhotoDocumentCompanyDTO.DocumentCompany.ContentType);
                    }
                }
            }

            /*=============================================
              ACTUALIZAMOS EL DOCUMENTO DE LA EMPRESA
             =============================================*/
            if (updatePhotoDocumentCompanyDTO.DocumentCompany != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatePhotoDocumentCompanyDTO.DocumentCompany.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(updatePhotoDocumentCompanyDTO.DocumentCompany.FileName);
                    urlDocumentCompanyFrontal = await storageArchives.EditArchive(contenido, $"document-company-{updatePhotoDocumentCompanyDTO.FirstName}-{updatePhotoDocumentCompanyDTO.LastName}-{randomString}", extension,
                        contenedor, updatePhotoDocumentCompanyDTO.UrlActualDocumentCompany, updatePhotoDocumentCompanyDTO.DocumentCompany.ContentType);
                }
            }


            var result = await context.Companies.SingleOrDefaultAsync(company => company.IdCompany == id);
            if (result != null)
            {
                result.DocumentCompany = urlDocumentCompanyFrontal;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        private static string RandomString(int length)
        {   Random random = new Random();
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
