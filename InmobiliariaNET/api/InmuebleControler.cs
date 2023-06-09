using System;
using System.Collections.Generic;
using PracticaMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PracticaMVC.api;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InmuebleController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public InmuebleController(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    [HttpPut("estado")]
    public IActionResult editarEstadoInmueble(EditViewInmueble inmuebleEditado){

        Propietario propietarioLogeado = ObtenerPropietarioLogueado();

        if(propietarioLogeado == null){
            return Unauthorized();
        }
        Inmueble inmuebleAModificar = _context.Inmuebles.Include(i => i.Duenio).First(i => i.Id == inmuebleEditado.id);
        if(inmuebleAModificar.Duenio.Id == propietarioLogeado.Id){
            inmuebleAModificar.Estado = inmuebleEditado.estado;
            return Ok(_context.SaveChanges());
        }else{
            return BadRequest("No puede modificar un inmueble que no sea suyo");
        }
        



    }
    [HttpGet("propiedades")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerPropiedades()
    {
        var propietarioActual = ObtenerPropietarioLogueado();

        if(propietarioActual == null)
        {
            return Unauthorized();
        }
        return propietarioActual == null
                                    ? Unauthorized()
                                    : Ok(_context.Inmuebles
                                               .Where(inmueble => inmueble.PropietarioId == propietarioActual.Id)
                                               .ToList());
    }


    [HttpGet("propiedades-alquiladas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerPropiedadesAlquiladas()
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
            return Unauthorized();

        var propiedadesAlquiladas = _context.Contratos
            .Where(contrato => contrato.Inmueble.Duenio.Id == propietarioActual.Id && contrato.Estado == true)
            .Select(contrato => contrato.Inmueble)
            .ToList();

        return Ok(propiedadesAlquiladas);
    }


    //ActualizarInmueble
    [HttpPut("actualizar-inmueble")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ActualizarInmueble([FromBody] Inmueble inmueble)
    {

        Propietario propietarioLogeado = ObtenerPropietarioLogueado();

        if(propietarioLogeado == null){
            return Unauthorized();
        }

        if (inmueble == null)
        {
            return BadRequest("No se proporcionó un inmueble válido.");
        }

        var inmuebleExistente = _context.Inmuebles.Include(i => i.Duenio).FirstOrDefault(i => i.Id == inmueble.Id);
        if (inmuebleExistente != null && inmuebleExistente.Duenio.Id == propietarioLogeado.Id)
        {
            // Actualizar los campos del inmueble existente con los valores proporcionados
            inmuebleExistente.PropietarioId = inmueble.PropietarioId;
            inmuebleExistente.Direccion = inmueble.Direccion;
            inmuebleExistente.Ambientes = inmueble.Ambientes;
            inmuebleExistente.Uso = inmueble.Uso;
            inmuebleExistente.Imagen = inmueble.Imagen;
            inmuebleExistente.Precio = inmueble.Precio;
            inmuebleExistente.Estado = inmueble.Estado;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok(inmuebleExistente);
        }

        return NotFound("No se encontró el inmueble especificado.");
    }





    private Propietario ObtenerPropietarioLogueado()
    {
        var email = User.Identity.Name;
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == email);
        return propietario;
    }



}