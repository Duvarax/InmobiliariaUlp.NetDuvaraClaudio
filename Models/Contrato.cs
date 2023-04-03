using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.Models;

public class Contrato
{   
    [Display(Name="Nro")]
    public int? Id { get; set; }

    [Display(Name ="Nro de Inquilino")]
    public int InquilinoId { get; set; }
    [ForeignKey(nameof(InquilinoId))]
    public Inquilino? Inquilino { get; set; }
    [Display(Name ="Nro de Inmueble")]
    public int? InmuebleId { get; set; }
    [ForeignKey(nameof(InmuebleId))]
    public Inmueble? Inmueble { get; set; }
    [Display(Name ="Inicio de contrato")]
    public DateTime? fechaInicio { get; set; }
    [Display(Name ="Finalizacion del contrato")]
    public DateTime? fechaFinalizacion { get; set; }
    public Double? Precio {get; set;}

    public Contrato()
    {
       
    }

}
