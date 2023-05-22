using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.api.Models;

public class LoginView
{   
    
    public string Email {get; set;}
    public string Contraseña {get; set;}

    public LoginView()
    {
       
    }

}
