namespace PracticaMVC.Models;

public class Persona
{   
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public DateTime? Nacimiento { get; set; }

    public Persona()
    {
        Nombre = "Juan";
    }

    public Persona(string nombre = ""){
        this.Nombre = nombre;
    }
    public string toString()
    {
        return Nombre;
    }
}
