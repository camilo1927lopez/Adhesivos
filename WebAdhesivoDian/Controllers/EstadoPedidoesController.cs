using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;

namespace WebAdhesivoDian.Controllers
{
    public class EstadoPedidoesController : Controller
    {
        private readonly WebAdhesivoDianContext _context;

        public EstadoPedidoesController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        // GET: EstadoPedidoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.EstadoPedido.ToListAsync());
        }

        // GET: EstadoPedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstadoPedido == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedido
                .FirstOrDefaultAsync(m => m.IdEstado == id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

            return View(estadoPedido);
        }

        // GET: EstadoPedidoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoPedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstado,Nombre,Descripcion")] EstadoPedido estadoPedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoPedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoPedido);
        }

        // GET: EstadoPedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstadoPedido == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedido.FindAsync(id);
            if (estadoPedido == null)
            {
                return NotFound();
            }
            return View(estadoPedido);
        }

        // POST: EstadoPedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstado,Nombre,Descripcion")] EstadoPedido estadoPedido)
        {
            if (id != estadoPedido.IdEstado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoPedidoExists(estadoPedido.IdEstado))
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
            return View(estadoPedido);
        }

        // GET: EstadoPedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstadoPedido == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedido
                .FirstOrDefaultAsync(m => m.IdEstado == id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

            return View(estadoPedido);
        }

        // POST: EstadoPedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstadoPedido == null)
            {
                return Problem("Entity set 'WebAdhesivoDianContext.EstadoPedido'  is null.");
            }
            var estadoPedido = await _context.EstadoPedido.FindAsync(id);
            if (estadoPedido != null)
            {
                _context.EstadoPedido.Remove(estadoPedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoPedidoExists(int id)
        {
          return _context.EstadoPedido.Any(e => e.IdEstado == id);
        }
    }
}
