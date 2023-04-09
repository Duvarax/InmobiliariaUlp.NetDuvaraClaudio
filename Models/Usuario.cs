using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaMVC.Models;

public enum enRoles
{
    Administrador = 1,
    Empleado = 2
}
public class Usuario
{   
    [Display(Name = "Nro")]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email {get; set;}
    public string NombreUsuario {get; set;}
    public string Contraseña { get; set; }
    public string? Avatar {get; set;}

    public IFormFile? AvatarFile{get; set;}
    
    public int Rol;
    


    public Usuario()
    {
       
    }

    public static IDictionary<int, string> getRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}

}
