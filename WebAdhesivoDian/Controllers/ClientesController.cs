using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Filters;
using WebAdhesivoDian.Models;
using WebAdhesivoDian.Repositories;

namespace WebAdhesivoDian.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly WebAdhesivoDianContext _context;
        private RepositoryWeb _repo;

        public ClientesController(WebAdhesivoDianContext context, RepositoryWeb repo)
        {
            _context = context;
            _repo = repo;   
        }


        //[AuthorizeUsers (Policy = "ADMINISTRADORES")]
        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Faltan campos obligatirio por ingresar");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    message = "El codigo de cliente ya se encuentra registrado";
                }
                ModelState.AddModelError(String.Empty,message);
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            try
            {
                if (id != cliente.IdCliente)
                {
                    throw new Exception("El cliente no existe");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cliente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!ClienteExists(cliente.IdCliente))
                        {
                            throw new Exception($"No puede actualizar el cliente: {ex.Message}");
                        }
                        else
                        {
                            throw new Exception($"No puede actualizar el cliente: {ex.Message}");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    message = "El codigo de el cliente ya se encuentra registrado";
                }
                else
                {

                }

                ModelState.AddModelError("", message);
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.IdCliente == id);
        }
    }
}
