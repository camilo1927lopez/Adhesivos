using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAdhesivoDian.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Código Cliente*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[^e0-9]+$|^[0-9]*$", ErrorMessage = "Ingrese solo números validos")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Nombre Cliente*")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Ultimo Pedido*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[^e0-9]+$|^[0-9]*$", ErrorMessage = "Ingrese solo números validos")]
        public int UltimoPedido { get; set; }

        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Consecutivo Cadena*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[^e0-9]+$|^[0-9]*$", ErrorMessage = "Ingrese solo números validos")]
        public int ConsecutivoCadena { get; set; }
        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Código de barra texto fijo*")]
        public string FormateoCodigoFijo { get; set; }
        public string Referencia { get; set; }
        public string Formato { get; set; }
        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Ruta de salida*")]
        public string RutaBaseSalida { get; set; }

        [Display(Name = "Estado*")]
        public bool Estado { get; set; }

        //Relaciones
        public List<Oficina> Oficinas { get; set; }
        public List<Encabezado> Encabezados { get; set; }
    }
}
