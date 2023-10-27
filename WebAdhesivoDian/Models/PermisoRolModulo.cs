using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdhesivoDian.Models
{
    [Table("PermisosRolModulo")]
    public class PermisoRolModulo
    {

        [Key]
        [Column("ID_Permiso")]
        public int Id_permiso { get; set; }

        [Column("Codigo_permiso")]
        public int Codigo_Permiso { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [ForeignKey(nameof(Roles))]
        [Column("Id_Rol")]
        public int Id_Rol { get; set; }

        [ForeignKey(nameof(Modulos))]
        [Column("Id_Modulo")]
        public int Id_Modulo { get; set; }

        public virtual Roles Roles { get; set; }
        public virtual Modulos Modulos { get; set; }

        

        
    }
}
