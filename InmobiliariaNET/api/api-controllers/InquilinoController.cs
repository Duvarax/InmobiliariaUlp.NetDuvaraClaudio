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
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public InquilinoController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this.contexto = context;
            this.config = config;
            this.environment = environment;
        }

        [HttpGet]
		public async Task<IActionResult> GetInquilinos()
		{
			
			int propietarioId = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
			return Ok(contexto.Inmuebles
				.Join(contexto.Contratos,
					inmueble => inmueble.Id,
					contrato => contrato.InmuebleId,
					(inmueble, contrato) => new { Inmueble = inmueble, Contrato = contrato })
				.Join(contexto.Inquilinos,
					inmuebleContrato => inmuebleContrato.Contrato.InquilinoId,
					inquilino => inquilino.Id,
					(inmuebleContrato, inquilino) => new { InmuebleContrato = inmuebleContrato, Inquilino = inquilino })
				.Where(joinResult => joinResult.InmuebleContrato.Contrato.Estado == true &&
									joinResult.InmuebleContrato.Contrato.Inmueble.PropietarioId == propietarioId)
				.Select(joinResult => joinResult.InmuebleContrato.Inmueble));
			
			
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetInquilinoById(int id){
			return Ok(contexto.Inquilinos.Where(i => i.Id == id));
		}

    }
