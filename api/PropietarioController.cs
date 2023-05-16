using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PracticaMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace PracticaMVC.api;

[Route("api/[controller]")]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> login([FromBody]LoginView login)
    {
        var key = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // string contraseñaHasheada = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        // 	password: login.Contraseña,
        // 	salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
        // 	prf: KeyDerivationPrf.HMACSHA1,
        // 	iterationCount: 1000,
        // 	numBytesRequested: 256/8
        // ));

        var propietario = await contexto.Propietarios.FirstOrDefaultAsync(p => p.Email == login.Email);

        if (propietario != null)
        {
            if (propietario.Clave == login.Contraseña)
            {
                var claims = new List<Claim>
                            {
                                new Claim("Id", propietario.Id+""),
                                new Claim(ClaimTypes.Name, propietario.Nombre),
                                new Claim("Email", propietario.Email),
                                new Claim("FullName", propietario.Nombre + " " + propietario.Email),
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
		return BadRequest();

    }

}
