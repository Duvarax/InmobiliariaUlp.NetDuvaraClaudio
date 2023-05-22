using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.api.Models;

public class Pago
{   
    [Display(Name="Nro Identificador")]
    public int? Id { get; set; }

    [Display(Name="Nro de pago")]
    public int? IdentificadorPago {get; set;}

    [Display(Name ="Fecha de pago")]
    public DateTime? fechaPago {get; set;}

    public Decimal? Importe {get; set;}

    [Display(Name ="Nro de Contrato")]
    public int? ContratoId {get; set;}
    [ForeignKey(nameof(ContratoId))]
    public Contrato? contrato {get; set;}

    public Boolean? Activo {get; set;}
    

    public Pago()
    {
       
    }

}
