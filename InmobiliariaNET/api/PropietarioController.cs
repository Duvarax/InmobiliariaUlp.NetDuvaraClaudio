using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PracticaMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
		public IActionResult Get()
		{
			int idPropietario = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
			return Ok(contexto.Propietarios.Where(p => p.Id == idPropietario));
			
			
		}


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginView loginView)
    {
        try
        {
            var usuario = contexto.Propietarios.FirstOrDefault(x => x.Email == loginView.Email);
            if (usuario == null)
            {
                return NotFound();
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginView.Contraseña,
                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            var p = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Email);
            if (p == null || p.Clave != hashed)
            {
                return BadRequest("Nombre de usuario o clave incorrecta");
            }
            else
            {
                var key = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {   
                    new Claim("Id", p.Id+""),
                    new Claim(ClaimTypes.Name, p.Email),
                    new Claim("FullName", p.Nombre + " " + p.Apellido),
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // GET api/propietario/perfil
    [HttpGet("perfil")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetUsuarioActual()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var emailClaim = identity.FindFirst(ClaimTypes.Name);
            var fullNameClaim = identity.FindFirst("FullName");
            var roleClaim = identity.FindFirst(ClaimTypes.Role);

            var email = emailClaim?.Value;
            var fullName = fullNameClaim?.Value;
            var role = roleClaim?.Value;

            var propietario = contexto.Propietarios.FirstOrDefault(x => x.Email == email);

            if (propietario != null)
            {
                return Ok(propietario);
            }
        }

        return Unauthorized();
    }

    [HttpPut("editar")]
    public IActionResult editarPerfil(Propietario propietarioEditado){


        contexto.Entry(propietarioEditado).State = EntityState.Modified;
        return Ok(contexto.SaveChanges());
    }


}

