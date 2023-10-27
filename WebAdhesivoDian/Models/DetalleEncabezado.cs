using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdhesivoDian.Models
{
    public class DetalleEncabezado
    {
        [Key]
        public int IdDetalleEncabezado { get; set; }
        public int IdEncabezado { get; set; }
        public int IdCajero { get; set; }
        [Required]
        public int CantidadInicial { get; set; }
        [Required]
        public int CantidadFinal { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int ConsectivoinicialCadena { get; set; }
        [Required]
        public int ConsectivoFinalCadena { get; set; }

        //Relacciones
        public Encabezado Encabezado { get; set; }
        public Cajero Cajero { get; set; }
    }
}
