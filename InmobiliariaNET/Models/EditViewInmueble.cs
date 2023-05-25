using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticaMVC.Models;

public class EditViewInmueble
{   
    
    public int id {get; set;}
    public Boolean estado {get; set;}

    public EditViewInmueble()
    {
       
    }

}
