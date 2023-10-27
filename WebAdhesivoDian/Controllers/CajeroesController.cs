using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebAdhesivoDian.Controllers
{
    [Authorize]
    public class CajeroesController : Controller
    {
        private readonly WebAdhesivoDianContext _context;

        public CajeroesController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        // GET: Cajeroes
        //public async Task<IActionResult> Index()
        //{
        //    var cajeros = await _context.Cajero.ToListAsync();
        //    foreach (var item in cajeros) 
        //    {
        //        Oficina oficina = _context.Oficina.Find(item.IdOficina);
        //        if (oficina.Estado)                 
        //            item.Oficina = oficina;
                
        //    }            
            

        //    return View(cajeros);
        //}

        public async Task<IActionResult> Index(int? id)
        {
            var cajero = new List<Cajero>();
            var oficinaBack = new List<Oficina>();
            if (id == null)
            {
                return RedirectToAction("Index", "Clientes");
                //cajero = await _context.Cajero.ToListAsync();

            }
            else
            {
                cajero = await _context.Cajero.Where(t => t.IdOficina == id).ToListAsync();

            }
            ViewBag.idOficina = id;
            foreach (var item in cajero)
            {
                Oficina oficina = _context.Oficina.Find(item.IdOficina);
                //if (oficina.Estado)
                    item.Oficina = oficina;
            }

            return View(cajero);
        }

        // GET: Cajeroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cajero = await _context.Cajero
                .FirstOrDefaultAsync(m => m.IdCajero == id);
            if (cajero == null)
            {
                return NotFound();
            }

            return View(cajero);
        }

        // GET: Cajeroes/Create
        public async  Task<IActionResult> Create(int? id)
        {
            Cajero cajero = new Cajero();
            Oficina oficina = _context.Oficina.Find(id);
            if (oficina.Estado)
                cajero.Oficina = oficina;
            CargarOficina(oficina);

            cajero.IdOficina = oficina.IdOficina;
            return View(cajero);
        }

        private void CargarOficina(Oficina oficina)
        {
            try
            {
                List<SelectListItem> ListaOficina = new List<SelectListItem>();
                ListaOficina.Add(new SelectListItem { Value = "00", Text = "Seleccionar Oficina" });

                List<Oficina> ltsOficinas = new List<Oficina>();
                ltsOficinas = _context.Oficina.Where(t => t.IdOficina == oficina.IdOficina).ToList();                
          

                foreach (var item in ltsOficinas)
                {
                    bool selecionado = false;
                    if (oficina != null)
                        if (oficina.IdOficina == item.IdOficina)
                            selecionado = true;

                    ListaOficina.Add(new SelectListItem { Value = item.IdOficina.ToString(), Text = $"{item.Codigo}-{item.Nombres}", Selected = selecionado });
                }

                ViewBag.ListaOficina = ListaOficina;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Cajeroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cajero cajero)
        {
            Oficina oficina = null;
            try
            {
                cajero.Oficina = new Oficina();

                if (cajero.IdOficina <= 0)
                    throw new Exception("Por favor seleccionar la oficina");

                

                oficina = _context.Oficina.Find(cajero.IdOficina);
                if (ModelState.IsValid)
                {
                    cajero.Oficina = null;
                    cajero.Estado = true;
                    _context.Add(cajero);
                    await _context.SaveChangesAsync();
                    return Redirect(Url.Action("Index", new { id = cajero.IdOficina }));
                    //return RedirectToAction(nameof(Index), new { id = cajero.IdOficina });
                }
                else
                {
                    CargarOficina(oficina);
                    ModelState.AddModelError(String.Empty, "Faltan campos obligatirio por ingresar");
                }
            }
            catch (Exception ex)
            {

                string message = ex.Message;
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    message = "El codigo del cajero ya se encuentra registrado";
                }
                else
                {

                }

               
             
                CargarOficina(oficina);
                ModelState.AddModelError("", message);
            }


            return View(cajero);
        }

        // GET: Cajeroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cajero = await _context.Cajero.FindAsync(id);
            if (cajero == null)
            {
                return NotFound();
            }

            cajero.Oficina = _context.Oficina.Find(cajero.IdOficina);
            CargarOficina(cajero.Oficina);

            
            return View(cajero);
        }

        // POST: Cajeroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cajero cajero)
        {
            try
            {

                if (cajero.IdOficina <= 0)
                    throw new Exception("Por favor seleccionar la oficina");

                if (id != cajero.IdCajero)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {

                        cajero.Oficina = _context.Oficina.Find(cajero.IdOficina);
                        _context.Update(cajero);

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!CajeroExists(cajero.IdCajero))
                        {
                            throw new Exception($"No puede actualizar el cajero: {ex.Message}");
                        }
                        else
                        {
                            throw new Exception($"No puede actualizar el cajero: {ex.Message}");
                        }
                    }
                    return Redirect(Url.Action("Index", new { id = cajero.IdOficina }));
                    //return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    message = "El codigo del cajero ya se encuentra registrado";
                }
                else
                {

                }
                
                ModelState.AddModelError("", message);
                
            }
            CargarOficina(cajero.Oficina);
            return View(cajero);
        }

        // GET: Cajeroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cajero = await _context.Cajero
                .FirstOrDefaultAsync(m => m.IdCajero == id);
            if (cajero == null)
            {
                return NotFound();
            }

            return View(cajero);
        }

        // POST: Cajeroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cajero = await _context.Cajero.FindAsync(id);
            _context.Cajero.Remove(cajero);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CajeroExists(int id)
        {
            return _context.Cajero.Any(e => e.IdCajero == id);
        }
    }
}
