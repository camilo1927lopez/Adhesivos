using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdhesivoDian.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(Roles))]
        [Display(Name = "Rol*")]
        [Column("ID_ROL")]
        [Required(ErrorMessage = "Por favor ingresa el {0} del usuario")]
        //[Required(ErrorMessage = "Porfavor selecciona el {0}")]
        public int IdRol { get; set; }

        [Column("PASS")]
        [Display(Name = "Contraseña*")]
        [Required(ErrorMessage = "Por favor ingresa la {0}")]
        //[DataType(DataType.Password)]
        public byte[] Password { get; set; }

        [Column("EMAIL")]
        [Display(Name = "Correo*")]
        [Required(ErrorMessage = "Por favor ingresa el {0}")]
        [EmailAddress(ErrorMessage = "El campo 'Correo' debe ser una dirección de correo electrónico válida.")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Por favor ingresa un Correo válido.")]
        public string Email { get; set; }

        [Column("SALT")]
        public string Salt { get; set; }

        [Column("NOMBRE")]
        [Display(Name = "Nombre*")]
        [Required(ErrorMessage = "Por favor ingresa el {0}")]
        public string Nombre { get; set; }

        [Column("APELLIDOS")]
        [Display(Name = "Apellidos*")]
        [Required(ErrorMessage = "Por favor ingresa los {0}")]
        public string Apellidos { get; set; }

        [Column("ESTADO")]
        [Display(Name = "Estado*")]
        [Required(ErrorMessage = "Por favor indica el {0}")]
        public bool Estado { get; set; }

        public virtual Roles Roles { get; set; }


    }
}