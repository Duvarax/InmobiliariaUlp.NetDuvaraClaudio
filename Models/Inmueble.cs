using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaMVC.Models;

public class Inmueble
{   
    [Display(Name = "Nro")]
    public int Id { get; set; }
    public string? Direccion { get; set; }
    public int? Ambientes { get; set; }
    public int? Superficie { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public int? PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio {get; set;}

    public decimal Precio {get; set;}
    public Boolean Estado {get; set;}

    public string? EstadoNombre => Estado == true ? "Disponible" : "No Disponible";
    

    public Inmueble()
    {
       
    }

}
