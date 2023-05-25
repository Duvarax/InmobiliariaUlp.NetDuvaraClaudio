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
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext _context;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public InquilinoController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this._context = context;
            this.config = config;
            this.environment = environment;
        }

        // [HttpGet]
		// public async Task<IActionResult> GetInquilinos()
		// {
			
		// 	int propietarioId = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
		// 	return Ok(_context.Inmuebles
		// 		.Join(_context.Contratos,
		// 			inmueble => inmueble.Id,
		// 			contrato => contrato.InmuebleId,
		// 			(inmueble, contrato) => new { Inmueble = inmueble, Contrato = contrato })
		// 		.Join(_context.Inquilinos,
		// 			inmuebleContrato => inmuebleContrato.Contrato.InquilinoId,
		// 			inquilino => inquilino.Id,
		// 			(inmuebleContrato, inquilino) => new { InmuebleContrato = inmuebleContrato, Inquilino = inquilino })
		// 		.Where(joinResult => joinResult.InmuebleContrato.Contrato.Estado == true &&
		// 							joinResult.InmuebleContrato.Contrato.Inmueble.PropietarioId == propietarioId)
		// 		.Select(joinResult => joinResult.InmuebleContrato.Inmueble));
			
			
		// }

		[HttpGet("{id}")]
		public async Task<IActionResult> GetInquilinoById(int id){
			return Ok(_context.Inquilinos.Where(i => i.Id == id));
		}

		//Dado un inmueble, retorna el inquilino del ultimo contrato activo de ese inmueble.
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerInquilinoUltimoContrato([FromBody] Inmueble inmueble)
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
            return Unauthorized();

        var inmuebleEncontrado = _context.Inmuebles
            .FirstOrDefault(i => i.Id == inmueble.Id && i.PropietarioId == propietarioActual.Id);

        if (inmuebleEncontrado == null)
            return NotFound();


        var contratoVigente = _context.Contratos
            .Include(c => c.Inquilino)
            .Where(contrato => contrato.InmuebleId == inmuebleEncontrado.Id && contrato.Estado == true)
            .OrderByDescending(contrato => contrato.fechaInicio)
            .FirstOrDefault();


        if (contratoVigente == null)
            return NotFound();


        return Ok(contratoVigente.Inquilino);
    }
	private Propietario ObtenerPropietarioLogueado()
    {
        var email = User.Identity.Name;
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == email);
        return propietario;
    }

    }
