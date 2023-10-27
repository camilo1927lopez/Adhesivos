using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdhesivoDian.Models
{
    public class Cajero
    {
        [Key]
        public int IdCajero { get; set; }


        [Required(ErrorMessage = "Por favor {0}")]
        [Display(Name = "Oficina*")]
        public int IdOficina { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Ultima Numeración Cajero*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Ingrese solo números validos")]
  
        public int ultimaNumeracion { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Código Cajero*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[^e0-9]+$|^[0-9]*$", ErrorMessage = "Ingrese solo números validos")]
        public string Codigo { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Direccion Cajero*")]
        public string Direccion { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Teléfono Oficina*")]
        [Range(0, 9999999999, ErrorMessage = "El número de teléfono no puede tener mas de 10 dígitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Ingrese solo números validos")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Nombre Cajero*")]
        public string Nombres { get; set; }  
        

        public bool Estado { get; set; }

        //Relaciones
        public Oficina Oficina { get; set; }
    }
}
