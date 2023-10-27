using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdhesivoDian.Models
{
    [Table("EstadoPedido")]
    public class EstadoPedido
    {
        [Key]
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

    }
}
