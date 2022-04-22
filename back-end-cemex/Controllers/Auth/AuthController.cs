using AutoMapper;
using back_end_cemex.DTOs;
using back_end_cemex.Entities;
using back_end_cemex.Helpers.Email;
using back_end_cemex.Helpers.Email.ForgotPassword;
using back_end_cemex.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace back_end_cemex.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;

        public AuthController(
             IMapper mapper,
            ApplicationDbContext context,
            UserManager<User> userManager, 
            IConfiguration configuration, SignInManager<User> signInManager,
            IEmailService emailService
            )
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ResponseAuth>> Register(CredentialsUser credentialsUser)
        {
            var user = new User {
                UserName = credentialsUser.Email,
                FirstName = credentialsUser.FirstName,
                LastName = credentialsUser.LastNanme,
                Document = credentialsUser.Document,
                Email = credentialsUser.Email,
                Status = true,
                PhoneNumber = credentialsUser.PhoneNumber,
                Slug = credentialsUser.FirstName,
            };
            var result = await userManager.CreateAsync(user, credentialsUser.Password);

            if (result.Succeeded)
            {
                if(credentialsUser.role == "ManTruck")
                {
                    await userManager.AddToRoleAsync(user, credentialsUser.role);
                    await userManager.AddToRoleAsync(user, "Driver");
                }
                await userManager.AddToRoleAsync(user, credentialsUser.role);
                /*List<string> roles = new List<string>();
                roles.Add("ManTruck");
                roles.Add("Driver");
                await userManager.AddToRolesAsync(user, roles);*/
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuth>> Login(RequestLogin requestLogin)
        {
            var user = await userManager.FindByEmailAsync(requestLogin.Email);
            var result = await signInManager.PasswordSignInAsync(
                requestLogin.Email, requestLogin.Password, 
                isPersistent:false, lockoutOnFailure:false                
                );

            if (result.Succeeded)
            {
                return await ContructTokenLogin(requestLogin, user);
            }
            else
            {
                /*List<string> errors = new List<string>();
                errors.Add("Estas credenciales no coinciden con nuestros registros");
                return BadRequest(new
                {
                    Errors = errors,
                }) ;*/
                return BadRequest("Estas credenciales no coinciden con nuestros registros");
     
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] RequestForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
                return BadRequest("El correo electrónico no coinciden con nuestros registros");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                { "token", token  },
                { "email", forgotPassword.Email },

            };

            var callback = QueryHelpers.AddQueryString(configuration["urlconsoleresetpassword"], param);
            var userForgotPassword = new UserForgotPassword
            {
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               TokenURL = callback,
               Token = token
            };
           var result =  emailService.SendUserEmail(userForgotPassword);

            if (result)
            {
                var data = new Dictionary<string, string>{
                    { "url", userForgotPassword.TokenURL },
                    { "email", userForgotPassword.Email },
                    { "token", userForgotPassword.Token },
                };
                return Ok(data);
                
            }else
            {
                return BadRequest();
            }          

        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseApi>> ResetPassword([FromBody] RequestResetPassword requestResetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.FindByEmailAsync(requestResetPassword.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var resetPassResult = await userManager.ResetPasswordAsync(user, requestResetPassword.Token, requestResetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            
            return new ResponseApi()
            {
                ok = true,
                message = "La contraseña se actualizo correctamente"
            };
        }

        private async Task<ActionResult<ResponseAuth>> ContructTokenLogin(RequestLogin requestLogin, User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", user.Email),
           
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);
            
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            var driver = await context.Drivers.FirstOrDefaultAsync(x => x.User == user);

            if(roles[0] == "AdminLogis")
            {
                var conveyor = await context.Conveyors.Include(with => with.TypeConveyor).Include(with => with.Company).FirstOrDefaultAsync(x => x.User == user);
                return new ResponseAuth()
                {
                    User = user,
                    Roles = (List<string>)roles,
                    CompanyId = conveyor.Company.IdCompany,
                    CompanyName = conveyor.Company.NameCompany,

                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion,

                };
            }

            if (roles[0] == "ManTruck")
            {
                var conveyor = await context.Conveyors.Include(with => with.TypeConveyor).Include(with => with.Company).FirstOrDefaultAsync(x => x.User == user);
                return new ResponseAuth()
                {
                    User = user,
                    Roles = (List<string>)roles,
                    IdDriver = driver.IdDriver,
                    CodeSap = driver.CodeSap,
                    Status = driver.Status,
                    DocumentDrivinglicenseFrontal = driver.DocumentDrivinglicenseFrontal,
                    DocumentDrivinglicenseBack = driver.DocumentDrivinglicenseBack,

                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion,

                };

            }

            return new ResponseAuth()
            {
                User = user,
                Roles = (List<string>)roles,
                IdDriver = driver.IdDriver,
                CodeSap = driver.CodeSap,
                Status = driver.Status,
                DocumentDrivinglicenseFrontal = driver.DocumentDrivinglicenseFrontal,
                DocumentDrivinglicenseBack = driver.DocumentDrivinglicenseBack,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,

            };
        }

        private ResponseAuth ContructToken(CredentialsUser credentialsUser)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credentialsUser.Email),
                new Claim("username", credentialsUser.FirstName),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer:null, audience:null, claims:claims,
                expires: expiracion, signingCredentials:creds);

            return new ResponseAuth()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
            };
        }
    }
}
