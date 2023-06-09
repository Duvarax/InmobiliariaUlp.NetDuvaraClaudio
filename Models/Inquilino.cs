using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace PracticaMVC.Models;

public class Inquilino
{   
    [Display(Name="Nro")]
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Dni { get; set; }
    public string? Email { get; set; }

    public Inquilino()
    {
       
    }

    public override string ToString()
    {
        return Nombre + " " + Apellido;
    }
}
