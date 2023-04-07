using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.Models;

public class Pago
{   
    [Display(Name="Nro")]
    public int? Id { get; set; }

    [Display(Name ="Fecha de pago")]
    public DateTime? fechaPago {get; set;}

    public Double? Importe {get; set;}

    public int? ContratoId {get; set;}
    [ForeignKey(nameof(ContratoId))]
    public Contrato? contrato {get; set;}
    

    public Pago()
    {
       
    }

}
