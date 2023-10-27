using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdhesivoDian.Models
{
    public class Oficina
    {
        [Key]
        public int IdOficina { get; set; }

        [Required(ErrorMessage = "Por favor {0}")]
        [Display(Name = "Cliente")]
        public int idCliente { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name ="Código Oficina*")]
        [Range(0, 999999999999999999, ErrorMessage = "Ingrese un número menor a 18 dígitos")]
        [RegularExpression("^[^e0-9]+$|^[0-9]*$", ErrorMessage = "Ingrese solo números validos")]

        public string Codigo { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Direccion Oficina*")]
        public string Direccion { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Teléfono Oficina*")]
        [Range(0, 9999999999, ErrorMessage = "El número de teléfono no puede tener mas de 10 dígitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Ingrese solo números validos")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "Por favor ingresar {0}")]
        [Display(Name = "Nombre Oficina*")]
        public string Nombres { get; set; }
        [Display(Name = "Código Dane Munucio")]
        public string CodigoMunucio { get; set; }
        [Display(Name = "Código Dane Cuidad")]
        public string CodigoCiudad { get; set; }
        [Display(Name = "Tipo Direccion Oficina")]
        public string TipoDireccion { get; set; }
        [Display(Name = "Destinatario Oficina")]
        public string Destinatario { get; set; }
        [Display(Name = "Reponsable1 Oficina")]
        public string Reponsable1 { get; set; }
        [Display(Name = "Reponsable2 Oficina")]
        public string Reponsable2 { get; set; }
        [Display(Name = "Reponsable3 Oficina")]
        public string Reponsable3 { get; set; }
        public bool Estado { get; set; }

        //Relaciones
        public Cliente Cliente { get; set; }
        public List<Cajero> Cajeros { get; set; }

    }
}
