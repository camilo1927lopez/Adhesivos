using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebAdhesivoDian.ModelsViews
{
    public class CargarArchivo
    {
        [Required(ErrorMessage = "Por favor {0}")]
        [Display(Name = "Seleccionar Archivo")]
        public IFormFile RutaArchivo { get; set; }


        public enum MyEnumCargarArchivo
        {
            CodigoCliente = 0,
            TipoPorducto = 1,
            CodigoOficina = 2, 
            CodigoCajero = 3,
            InicioNumeracion = 4,
            CantidadNumeracion = 5,
            InfoCliente = 6
        }
    }
}
