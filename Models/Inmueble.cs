namespace PracticaMVC.Models;

public class Inmueble
{   
    public int Id { get; set; }
    public string? Direccion { get; set; }
    public int? Ambientes { get; set; }
    public int? Superficie { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public int? PropietarioId { get; set; }
    public Propietario Duenio {get; set;}

    public Inmueble()
    {
       
    }

    public override string ToString()
    {
        return Direccion + " " + Duenio.ToString();
    }
}
