using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PracticaMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace PracticaMVC.api;

	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly DataContext _context;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public ContratoController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this._context = context;
            this.config = config;
            this.environment = environment;
        }

        //Dado un inmueble retorna el contrato activo de dicho inmueble
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ObtenerContratoVigente([FromBody] Inmueble inmueble)
		{
			var propietarioActual = ObtenerPropietarioLogueado();
			if (propietarioActual == null)
			{
				return Unauthorized("No se encontró un propietario autenticado.");
			}

			var inmuebleEncontrado = _context.Inmuebles.FirstOrDefault(i => i.Id == inmueble.Id && i.PropietarioId == propietarioActual.Id);
			if (inmuebleEncontrado == null)
			{
				return NotFound("No se encontró el inmueble especificado para el propietario actual.");
			}

			var contratoVigente = _context.Contratos
				.Include(c => c.Inquilino)
				.FirstOrDefault(contrato => contrato.InmuebleId == inmuebleEncontrado.Id && contrato.Estado == true);

			if (contratoVigente == null)
			{
				return NotFound("No se encontró un contrato vigente para el inmueble especificado.");
			}

			return Ok(contratoVigente);
		}
		private Propietario ObtenerPropietarioLogueado()
    {
        var email = User.Identity.Name;
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == email);
        return propietario;
    }


    }