using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdhesivoDian.Helpers;
using WebAdhesivoDian.ModelsViews;
using System.Text;

namespace WebAdhesivoDian.Repositories
{
    public class RepositoryWeb
    {
        private WebAdhesivoDianContext context;

        public RepositoryWeb(WebAdhesivoDianContext context)
        {
            this.context = context;
        }

        private int GetMaxIdUsuario()
        {
            if (this.context.Usuario.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuario.Max(z => z.IdUsuario) + 1;
            }
        }

        private bool ExisteEmail(string email)
        {
            var consulta = from datos in this.context.Usuario
                           where datos.Email == email
                           select datos;
            if (consulta.Count() > 0)
            {
                //El email existe en la base de datos
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos, int idRol, bool estado)
        {
            try
            {
                bool ExisteEmail = this.ExisteEmail(email);
                if (ExisteEmail)
                {
                    throw new Exception($"El Email {email} ya se encuentra registrado");
                    
                }
                else
                {
                    int idusuario = this.GetMaxIdUsuario();
                    Usuario usuario = new Usuario();

                    usuario.Email = email;
                    usuario.Nombre = nombre;
                    usuario.Apellidos = apellidos;
                    usuario.IdRol = Convert.ToInt32(idRol);
                    usuario.Estado = true;
                    //GENERAMOS UN SALT ALEATORIO PARA CADA USUARIO
                    usuario.Salt = HelperCryptography.GenerateSalt();
                    //GENERAMOS SU PASSWORD CON EL SALT
                    usuario.Password = HelperCryptography.EncriptarPassword(password, usuario.Salt);
                    this.context.Usuario.Add(usuario);
                    this.context.SaveChanges();

                    return true;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           

        }

        public  bool EditarUsuario(Usuario usuario)
        {
            
            try
            {
                
                //int idusuario = this.GetMaxIdUsuario();
                //Usuario usuario = new Usuario();
                //usuario.Email = email;
                //usuario.Nombre = nombre;
                //usuario.Apellidos = apellidos;
                //usuario.IdRol = idRol;
                //usuario.Estado = estado;
                //usuario.IdUsuario = idUsuario;
                //GENERAMOS UN SALT ALEATORIO PARA CADA USUARIO
                usuario.Salt = HelperCryptography.GenerateSalt();
                //GENERAMOS SU PASSWORD CON EL SALT
                
                usuario.Password = HelperCryptography.EncriptarPassword(Encoding.UTF8.GetString(usuario.Password), usuario.Salt);
                this.context.Usuario.Update(usuario);
                this.context.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
            
               
            

        }
       

        public Usuario LogInUsuario(string email, string password)
        {

            Usuario usuario = this.context.Usuario.SingleOrDefault(x => x.Email == email);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                //Debemos comparar con la base de datos el password haciendo de nuevo el cifrado con cada salt de usuario
                byte[] passUsuario = usuario.Password;
                string salt = usuario.Salt;
                //Ciframos de nuevo para comparar
                byte[] temporal = HelperCryptography.EncriptarPassword(password, salt);

                //Comparamos los arrays para comprobar si el cifrado es el mismo
                bool respuesta = HelperCryptography.compareArrays(passUsuario, temporal);
                if (respuesta == true)
                {
                    usuario.Password = HelperCryptography.EncriptarPassword(password,salt);
                    return usuario;
                }
                else
                {
                    //Contraseña incorrecta
                    return null;
                }
            }
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this.context.Usuario
                           select datos;
            return consulta.ToList();
        }


    }
}