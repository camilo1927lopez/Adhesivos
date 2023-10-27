using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdhesivoDian.Models;

namespace WebAdhesivoDian.ModelsViews
{
    public class UsuarioView : Usuario
    {
       
        [Required(ErrorMessage = "Por favor ingresa la {0}*")]
        [Compare(nameof(Password),ErrorMessage = "Las contraseñas no coinciden, por favor verificar.")]
        [Display(Name = "Confirmar Contraseña*")]
        //[DataType(DataType.Password)]
        public byte[] passwordConfirm { get; set; }


       

    }
}
