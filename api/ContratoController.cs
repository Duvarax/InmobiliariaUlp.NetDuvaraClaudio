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
    public class ContratoController : ControllerBase
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

        public ContratoController(DataContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            this.contexto = context;
            this.config = config;
            this.environment = environment;
        }

        // [HttpGet]
		// public async Task<IActionResult> GetContratos()
		// {
			
		// 	int propietarioId = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
		// 	return Ok(contexto.Contratos.Where(c));
			
			
		// }

		[HttpGet("{id}")]
		public async Task<IActionResult> GetInquilinoById(int id){
			return Ok(contexto.Inquilinos.Where(i => i.Id == id));
		}

    }
