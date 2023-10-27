using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebAdhesivoDian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAdhesivoDian.Repositories;
using WebAdhesivoDian.Helpers;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Filters;
using Microsoft.AspNetCore.Authorization;
using WebAdhesivoDian.ModelsViews;
using System.Text;
using System.Security.Cryptography;
using BCryptNet = BCrypt.Net.BCrypt;

namespace WebAdhesivoDian.Controllers
{

    public class UsuariosController : Controller
    {
        private WebAdhesivoDianContext _context;
        private RepositoryWeb repo;

        public UsuariosController(RepositoryWeb repo, WebAdhesivoDianContext context)
        {
            this.repo = repo;
            this._context = context;
        }

        // GET: Roles
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Usuario> ListaUsuarios = new List<Usuario>();
            ListaUsuarios = await _context.Usuario.Include(t => t.Roles!).ToListAsync();

            return View(ListaUsuarios);
        }

        public async Task<IActionResult> Login()
        {
            return View(_context.Roles.ToList());
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            byte[] bytes = Convert.FromBase64String(password); // Convierte el dato a un arreglo de bytes
            password = System.Text.Encoding.UTF8.GetString(bytes);

            Usuario usuario = this.repo.LogInUsuario(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "El usuario ingresado no existe, o estás ingresando credenciales incorrectas";
                return View();
            }
            else
            {

                //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                //Y UN PRINCIPAL
                //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                //AUTENTIFICACION
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, _context.Roles.FirstOrDefault(t => t.Id == usuario.IdRol).Nombre);
                Claim claimIdUsuario = new Claim("IdUsuario", usuario.IdUsuario.ToString());
                Claim claimEmail = new Claim("EmailUsuario", usuario.Email);


                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);


                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(5),
                    AllowRefresh = true,
                });

                return RedirectToAction("Index", "Encabezadoes");
            }

        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("Login");
        }

        private bool ExisteEmail(string email)
        {
            var consulta = from datos in this._context.Usuario
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Password, string passwordConfirm, string Nombre, string Apellidos, int IdRol, int IdUsuario, bool Estado)
        {
            byte[] bytes = Convert.FromBase64String(Password); // Convierte el dato a un arreglo de bytes
            Password = System.Text.Encoding.UTF8.GetString(bytes);


            byte[] bytes2 = Convert.FromBase64String(passwordConfirm); // Convierte el dato a un arreglo de bytes
            passwordConfirm = System.Text.Encoding.UTF8.GetString(bytes);

            UsuarioView user = new UsuarioView();
            try
            {



                var Usuarios = await _context.Usuario.FindAsync(id);
                bool contraseñaConfirm = true;
                bool RolConfirm = true;

                //user.IdUsuario = Usuarios.IdUsuario;
                //user.Email = Usuarios.Email;
                //user.Nombre = Nombre;
                //user.Apellidos = Apellidos;
                //user.Estado = Estado;
                //user.IdRol = IdRol;

                Usuarios.Password = Encoding.UTF8.GetBytes(Password);
                Usuarios.Nombre = Nombre;
                Usuarios.Apellidos = Apellidos;
                Usuarios.IdRol = IdRol;
                Usuarios.Estado = Estado;





                List<Roles> ListaRoles = new List<Roles>();
                ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
                ListaRoles.AddRange(_context.Roles.ToList());

                ViewBag.ListaRoles = ListaRoles;


                if (IdRol == 0)
                {
                    ModelState.AddModelError(String.Empty, "El Rol es obligatorio");
                    RolConfirm = false;
                }

                if (contraseñaConfirm && RolConfirm)
                {
                    bool editado = this.repo.EditarUsuario(Usuarios);
                    return RedirectToAction("Index", "Usuarios");
                }



            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    message = "El codigo de la oficina ya se encuentra registrado";
                }
                else
                {

                }

                ModelState.AddModelError("", message);

            }
            return View(user);

        }


        [HttpPost]
        public IActionResult Registro(string Email, string Password, string Nombre, string Apellidos, int IdRol, bool Estado)
        {
            UsuarioView user = new UsuarioView();
            bool contraseñaConfirm = true;
            bool RolConfirm = true;

            try
            {
                user.Email = Email;
                user.Nombre = Nombre;
                user.Apellidos = Apellidos;
                user.Estado = Estado;
                user.IdRol = IdRol;
                user.Estado = true;

                List<Roles> ListaRoles = new List<Roles>();
                ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
                ListaRoles.AddRange(_context.Roles.ToList());

                ViewBag.ListaRoles = ListaRoles;

                if (Password.Length <= 7)
                {
                    ModelState.AddModelError(String.Empty, "Por favor ingresa una contraseña con 8 caracteres o más");
                    contraseñaConfirm = false;
                }

                if (IdRol == 0)
                {
                    ModelState.AddModelError(String.Empty, "El Rol es obligatorio");
                    RolConfirm = false;
                }

                if (contraseñaConfirm && RolConfirm)
                {
                    Estado = true;
                    bool registrado = this.repo.RegistrarUsuario(Email, Password, Nombre, Apellidos, IdRol, Estado);
                    return RedirectToAction("Index", "Usuarios");

                }
                contraseñaConfirm = true;
                RolConfirm = true;





            }
            catch (Exception ex)
            {
                string message = ex.Message;


                ModelState.AddModelError("", message);
            }

            return View("Create", user);
            //return RedirectToAction("Create", user);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuario.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            List<Roles> ListaRoles = new List<Roles>();
            ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
            ListaRoles.AddRange(_context.Roles.ToList());
            UsuarioView usuarioView = new UsuarioView();

            usuarioView.Email = usuarios.Email;
            usuarioView.Nombre = usuarios.Nombre;
            usuarioView.Apellidos = usuarios.Apellidos;
            usuarioView.IdRol = usuarios.IdRol;
            usuarioView.Estado = usuarios.Estado;
            usuarioView.IdUsuario = usuarios.IdUsuario;





            ViewBag.ListaRoles = ListaRoles;
            return View(usuarioView);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id,UsuarioView usuarioView)
        //{
        //    try
        //    {
        //        if (id != usuarioView.IdUsuario)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {


        //                bool editado = this.repo.EditarUsuario(usuarioView);
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!UsuarioExists(usuarioView.IdUsuario))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        List<Roles> ListaRoles = new List<Roles>();
        //        ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
        //        ListaRoles.AddRange(_context.Roles.ToList());
        //        ViewBag.ListaRoles = ListaRoles;
        //        return View(usuarioView);

        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //        if (ex.InnerException!.Message.Contains("duplicate"))
        //        {
        //            message = "El codigo de la oficina ya se encuentra registrado";
        //        }
        //        else
        //        {

        //        }

        //        ModelState.AddModelError("", message);
        //        return View(usuarioView);
        //    }

        //}

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.IdUsuario == id);
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuario.Any(e => e.IdUsuario == id);
        }
        
        public IActionResult Create()
        {
            List<Roles> ListaRoles = new List<Roles>();
            ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
            ListaRoles.AddRange(_context.Roles.ToList());
            UsuarioView usuarioView = new UsuarioView();



            ViewBag.ListaRoles = ListaRoles;
            return View(new UsuarioView());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(UsuarioView usuario)
        {

            try
            {
                List<Roles> ListaRoles = new List<Roles>();
                ListaRoles.Add(new Roles { Id = 0, Nombre = "Seleccionar Rol" });
                ListaRoles.AddRange(_context.Roles.ToList());

                ViewBag.ListaRoles = ListaRoles;

                if (ModelState.IsValid)
                {
                    usuario.Estado = true;
                    bool registrado = this.repo.RegistrarUsuario(usuario.Email, usuario.Password.ToString(), usuario.Nombre, usuario.Apellidos, usuario.IdRol, usuario.Estado);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Faltan campos obligatirio por ingresar");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(usuario);
        }




    }
}