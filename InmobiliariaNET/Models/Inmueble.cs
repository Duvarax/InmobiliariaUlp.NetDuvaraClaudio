using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaMVC.Models;

public class Inmueble
{   
    [Display(Name = "Nro")]
    public int Id { get; set; }
    public string? Direccion { get; set; }
    public int? Ambientes { get; set; }
    public string? Uso {get; set;}
    public string? Tipo {get; set;}

    public string? Imagen { get; set; }
    public int? PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio {get; set;}

    public Double Precio {get; set;}
    public Boolean Estado {get; set;}

    [NotMapped]
    public IFormFile ImagenFile {get; set;}

    public string? EstadoNombre => Estado == true ? "Disponible" : "No Disponible";
    

    public Inmueble()
    {
       
    }

}
