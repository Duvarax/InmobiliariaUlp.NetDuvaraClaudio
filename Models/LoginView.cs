using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.Models;

public class LoginView
{   
    
    public string Email {get; set;}
    public string Contraseña {get; set;}

    public LoginView()
    {
       
    }

}
