using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;

namespace WebAdhesivoDian.Controllers
{
    [Authorize]
    public class PermisoRolModuloesController : Controller
    {
       
        private readonly WebAdhesivoDianContext _context;

        public PermisoRolModuloesController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        // GET: PermisoRolModuloes
        //public async Task<IActionResult> Index()
        //{
        //    var webAdhesivoDianContext = _context.PermisoRolModulo.Include(p => p.Modulos).Include(p => p.Roles);
        //    return View(await webAdhesivoDianContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(int? id)
        {
            var permiso = new List<PermisoRolModulo>();
            if (id == null)
            {
                permiso = await _context.PermisoRolModulo.ToListAsync();

            }
            else
            {
                permiso = await _context.PermisoRolModulo.Where(t => t.Id_Rol == id).ToListAsync();

            }
            ViewBag.ListaRol = permiso;
            foreach (var item in permiso)
            {
                Roles rol = _context.Roles.Find(item.Id_Rol);
                Modulos modulo = _context.Modulos.Find(item.Id_Modulo);
                if (rol.Estado)
                    item.Roles = rol;
                    item.Modulos = modulo;

            }

            return View(permiso);
        }

        // GET: PermisoRolModuloes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PermisoRolModulo == null)
            {
                return NotFound();
            }

            var permisoRolModulo = await _context.PermisoRolModulo
                .Include(p => p.Modulos)
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(m => m.Id_permiso == id);
            if (permisoRolModulo == null)
            {
                return NotFound();
            }

            return View(permisoRolModulo);
        }

        // GET: PermisoRolModuloes/Create
        public IActionResult Create()
        {

            var cadena = _context.Modulos.Select(t => new { Id_Modulo = t.Id_Modulo, Descripcion = string.Concat(t.Nombre_Modulo, " [", t.Accion, "]") });
            ViewData["Id_Modulo"] = new SelectList(cadena, "Id_Modulo", "Descripcion", cadena.FirstOrDefault().Id_Modulo); ; ;
            ViewData["Id_Rol"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: PermisoRolModuloes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_permiso,Codigo_Permiso,Descripcion,Id_Rol,Id_Modulo")] PermisoRolModulo permisoRolModulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permisoRolModulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var cadena =  _context.Modulos.Select(t => new { Id_Modulo = t.Id_Modulo, Descripcion = string.Concat(t.Nombre_Modulo, " [", t.Accion, "]") });
            ViewData["Id_Modulo"] = new SelectList(cadena, "Id_Modulo", "Descripcion", cadena.FirstOrDefault().Id_Modulo); ; ;
            ViewData["Id_Rol"] = new SelectList(_context.Roles, "Id", "Nombre", permisoRolModulo.Id_Rol);
            return View(permisoRolModulo);
        }

        // GET: PermisoRolModuloes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PermisoRolModulo == null)
            {
                return NotFound();
            }

            var permisoRolModulo = await _context.PermisoRolModulo.FindAsync(id);
            if (permisoRolModulo == null)
            {
                return NotFound();
            }
            var cadena = _context.Modulos.Select(t => new { Id_Modulo = t.Id_Modulo, Descripcion = string.Concat(t.Nombre_Modulo, " [", t.Accion, "]") });
            ViewData["Id_Modulo"] = new SelectList(cadena, "Id_Modulo", "Descripcion", cadena.FirstOrDefault().Id_Modulo); ; ;
            ViewData["Id_Rol"] = new SelectList(_context.Roles, "Id", "Nombre", permisoRolModulo.Id_Rol);
            return View(permisoRolModulo);
        }

        // POST: PermisoRolModuloes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_permiso,Codigo_Permiso,Descripcion,Id_Rol,Id_Modulo")] PermisoRolModulo permisoRolModulo)
        {
            if (id != permisoRolModulo.Id_permiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permisoRolModulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoRolModuloExists(permisoRolModulo.Id_permiso))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_Modulo"] = new SelectList(_context.Modulos, "Id_Modulo", "Id_Modulo", permisoRolModulo.Id_Modulo);
            ViewData["Id_Rol"] = new SelectList(_context.Roles, "Id", "Id", permisoRolModulo.Id_Rol);
            return View(permisoRolModulo);
        }

        // GET: PermisoRolModuloes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PermisoRolModulo == null)
            {
                return NotFound();
            }

            var permisoRolModulo = await _context.PermisoRolModulo
                .Include(p => p.Modulos)
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(m => m.Id_permiso == id);
            if (permisoRolModulo == null)
            {
                return NotFound();
            }

            return View(permisoRolModulo);
        }

        // POST: PermisoRolModuloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PermisoRolModulo == null)
            {
                return Problem("Entity set 'WebAdhesivoDianContext.PermisoRolModulo'  is null.");
            }
            var permisoRolModulo = await _context.PermisoRolModulo.FindAsync(id);
            if (permisoRolModulo != null)
            {
                _context.PermisoRolModulo.Remove(permisoRolModulo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoRolModuloExists(int id)
        {
          return _context.PermisoRolModulo.Any(e => e.Id_permiso == id);
        }

        private void CargarRol(Roles rol)
        {
            try
            {
                List<SelectListItem> ListaRol = new List<SelectListItem>();
                ListaRol.Add(new SelectListItem { Value = "00", Text = "Seleccionar Rol" });

                List<Roles> ltsRoles = new List<Roles>();
                ltsRoles = _context.Roles.Where(t => t.Id == rol.Id).ToList();


                foreach (var item in ltsRoles)
                {
                    bool selecionado = false;
                    if (rol != null)
                        if (rol.Id == item.Id)
                            selecionado = true;

                    ListaRol.Add(new SelectListItem { Value = item.Id.ToString(), Text = $"{item.Id}-{item.Nombre}", Selected = selecionado });
                }

                ViewBag.ListaRol = ListaRol;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
