using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdhesivoDian.Models
{
    [Table("Modulos")]
    public class Modulos
    {
        [Key]
        [Column("ID_Modulos")]
        public int Id_Modulo { get; set; }

        [Column("CODIGO")]
        public int Codigo_modulo { get; set; }

        [Column("NOMBRE_MODULO")]
        public string Nombre_Modulo { get; set; }

        [Column("ACCION_MODULO")]
        public string Accion { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }
    }
}
