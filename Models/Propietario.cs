namespace PracticaMVC.Models;

public class Propietario
{   
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Dni { get; set; }
    public string? Email { get; set; }
    public string? Clave {get; set;}


    public Propietario()
    {
       
    }

    public override string ToString()
    {
        return Nombre + " " + Apellido;
    }
}
