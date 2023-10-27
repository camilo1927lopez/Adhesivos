using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdhesivoDian.Models
{
    public class Encabezado
    {
        [Key]
        public int IdEncabezado { get; set; }

        [ForeignKey(nameof(EstadoPedido))]
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        public int IdCliente { get; set; }
        [Required]
        public string NombrePedido { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public string NombreArchivoCargado { get; set; }

        

        //Relacciones
        public virtual ICollection<DetalleEncabezado> DetalleEncabezados { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoPedido EstadoPedido { get; set; }
       

    }
}
