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
    public class ModulosController : Controller
    {
        private readonly WebAdhesivoDianContext _context;

        public ModulosController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        // GET: Modulos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Modulos.ToListAsync());
        }

        // GET: Modulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Modulos == null)
            {
                return NotFound();
            }

            var modulos = await _context.Modulos
                .FirstOrDefaultAsync(m => m.Id_Modulo == id);
            if (modulos == null)
            {
                return NotFound();
            }

            return View(modulos);
        }

        // GET: Modulos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Modulo,Codigo_modulo,Nombre_Modulo,Accion,Descripcion,Estado")] Modulos modulos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modulos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modulos);
        }

        // GET: Modulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Modulos == null)
            {
                return NotFound();
            }

            var modulos = await _context.Modulos.FindAsync(id);
            if (modulos == null)
            {
                return NotFound();
            }
            return View(modulos);
        }

        // POST: Modulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Modulo,Codigo_modulo,Nombre_Modulo,Accion,Descripcion,Estado")] Modulos modulos)
        {
            if (id != modulos.Id_Modulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modulos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulosExists(modulos.Id_Modulo))
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
            return View(modulos);
        }

        // GET: Modulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Modulos == null)
            {
                return NotFound();
            }

            var modulos = await _context.Modulos
                .FirstOrDefaultAsync(m => m.Id_Modulo == id);
            if (modulos == null)
            {
                return NotFound();
            }

            return View(modulos);
        }

        // POST: Modulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Modulos == null)
            {
                return Problem("Entity set 'WebAdhesivoDianContext.Modulos'  is null.");
            }
            var modulos = await _context.Modulos.FindAsync(id);
            if (modulos != null)
            {
                _context.Modulos.Remove(modulos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulosExists(int id)
        {
          return _context.Modulos.Any(e => e.Id_Modulo == id);
        }
    }
}
