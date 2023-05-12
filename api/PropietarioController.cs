using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PracticaMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
namespace PracticaMVC.api;

	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class PropietarioController : ControllerBase
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public PropietarioController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this.contexto = context;
            this.config = config;
            this.environment = environment;
        }

        [HttpGet]
		public List<Propietario> Get()
		{
			
			return contexto.Propietarios.ToList();
			
			
		}

        [HttpGet("token")]
        [AllowAnonymous]
        public async Task<IActionResult> login(){
            var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, "DUVARAX@GMAIL.COM"),
						new Claim("FullName", "Duvara" + " " + "Claudio"),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
