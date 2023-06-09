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
    public class PagoController : ControllerBase
    {
        private readonly DataContext _context;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public PagoController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this._context = context;
            this.config = config;
            this.environment = environment;
        }

		//Dado un Contrato, retorna los pagos de dicho contrato
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ObtenerPagosContrato([FromBody] Contrato contratoVer)
		{
			var propietarioActual = ObtenerPropietarioLogueado();
			if (propietarioActual == null)
				return Unauthorized();

			if (contratoVer == null)
				return BadRequest("No se proporcionó un contrato válido.");

			var pagosContrato = _context.Pagos
				.Where(pago => pago.ContratoId == contratoVer.Id && pago.contrato.Inmueble.PropietarioId == propietarioActual.Id)
				.ToList();

			return Ok(pagosContrato);
		}

		private Propietario ObtenerPropietarioLogueado()
    {
        var email = User.Identity.Name;
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == email);
        return propietario;
    }

    }
