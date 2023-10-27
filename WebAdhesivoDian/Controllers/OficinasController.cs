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
    public class OficinasController : Controller
    {
        private readonly WebAdhesivoDianContext _context;

        public OficinasController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        // GET: Oficinas
        //public async Task<IActionResult> Index()
        //{
        //    var oficinas = await _context.Oficina.ToListAsync();
        //    foreach (var item in oficinas) 
        //    {
        //        Cliente cliente = _context.Cliente.Find(item.idCliente);
        //        if (cliente.Estado)                
        //            item.Cliente = cliente;
                
        //    }            
            
        //    return View(oficinas);
        //}

        public async Task<IActionResult> Index(int? id)
        {
            var oficinas = new List<Oficina>();
            var OficinasBack = new List<Oficina>();
            int idOficinaBack= 0;
            var ListOficinasBack = new List<Oficina>();
            if (id == null)
            {

                
                return RedirectToAction("Index", "Clientes");
                //oficinas = await _context.Oficina.ToListAsync();

            }
            else
            {
                oficinas = await _context.Oficina.Where(t => t.idCliente == id).ToListAsync();

            }

            if (oficinas.Count == 0)
            {
                OficinasBack = await _context.Oficina.Where(t => t.IdOficina == id).ToListAsync();
                idOficinaBack = OficinasBack.FirstOrDefault().idCliente;
                ListOficinasBack = await _context.Oficina.Where(t => t.idCliente == idOficinaBack).ToListAsync();

                ViewBag.IdCliente = idOficinaBack;
                foreach (var item in ListOficinasBack)
                {
                    Cliente cliente = _context.Cliente.Find(idOficinaBack);
                    //if (cliente.Estado)
                    item.Cliente = cliente;

                }
                return View(ListOficinasBack);

            }
            else {

                ViewBag.IdCliente = id;
                foreach (var item in oficinas)
                {
                    Cliente cliente = _context.Cliente.Find(id);
                    //if (cliente.Estado)
                    item.Cliente = cliente;


                }
                return View(oficinas);
            }

           

           
        }

        // GET: Oficinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oficina = await _context.Oficina
                .FirstOrDefaultAsync(m => m.IdOficina == id);
            if (oficina == null)
            {
                return NotFound();
            }

            return View(oficina);
        }

        

        // GET: Oficinas/Create
        //public IActionResult Create()
        //{
        //    cargarCleinte(null);
        //    Oficina oficina = new Oficina();
        //    oficina.Cliente = new Cliente();
        //    return View(oficina);
        //}

        public async Task<IActionResult> Create(int? id)
        {
            Oficina oficina = new Oficina();
            Cliente cliente = _context.Cliente.Find(id);
            if (cliente.Estado)
                oficina.Cliente = cliente;
            cargarCleinte(cliente);

            oficina.idCliente = cliente.IdCliente;
            return View(oficina);
        }

        private void cargarCleinte(Cliente cliente)
        {
            
            try
            {
                List<SelectListItem> ListaCliente = new List<SelectListItem>();
                ListaCliente.Add(new SelectListItem { Text = "Seleccionar Cliente", Value = "0"});

                List<Cliente> ltsCliente = new List<Cliente>();
                ltsCliente = _context.Cliente.Where(t => t.IdCliente == cliente.IdCliente).ToList();
                foreach (var item in ltsCliente)
                {
                    bool selecionado = false;
                    if (cliente != null)                    
                        if (cliente.IdCliente == item.IdCliente)                       
                            selecionado = true;                                            

                    ListaCliente.Add(new SelectListItem { Text = $"{item.Codigo}-{item.Nombre}", Value = item.IdCliente.ToString(), Selected = selecionado });
                }

                ViewBag.ListaCliente = ListaCliente;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        // POST: Oficinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, Oficina oficina)
        {
            Cliente cliente = null;
            try
            {
                oficina.Cliente = new Cliente();
                oficina.idCliente = (int)id; 
                if (oficina.idCliente <= 0)
                    throw new Exception("Por favor seleccionar el cliente");

                cliente = _context.Cliente.Find(oficina.idCliente);

                if (ModelState.IsValid)
                {
                    oficina.Estado = true;
                    oficina.Cliente = null;
                    _context.Add(oficina);
                    await _context.SaveChangesAsync();
                    //return Redirect(Url.Action("Index",oficina));
                    //return RedirectToAction(nameof(Index));
                    //return View("Index",oficina);
                    return Redirect(Url.Action("Index", new { id = oficina.idCliente }));
                }
                else
                {
                    cargarCleinte(cliente);
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
                else {
                
                }
                
                cargarCleinte(cliente);
                ModelState.AddModelError("", message);
            }
            return View( oficina);
        }

        // GET: Oficinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oficina = await _context.Oficina.FindAsync(id);
            if (oficina == null)
            {
                return NotFound();
            }
            oficina.Cliente = _context.Cliente.Find(oficina.idCliente);
            cargarCleinte(oficina.Cliente);

            return View(oficina);
        }

        // POST: Oficinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Oficina oficina)
        {
            try      
            {
                //var ofi = await _context.Oficina.FindAsync(id);
                //oficina.idCliente = ofi.idCliente;
                if (oficina.idCliente <= 0)
                    throw new Exception("Por favor seleccionar la oficina");

                

                if (ModelState.IsValid)
                {
                    try
                    {
                        oficina.Cliente = _context.Cliente.Find(oficina.idCliente);
                        _context.Update(oficina);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!OficinaExists(oficina.IdOficina))
                        {
                            throw new Exception($"No puede actualizar el oficina: {ex.Message}");
                        }
                        else
                        {
                            throw new Exception($"No puede actualizar el oficina: {ex.Message}");
                        }
                    }
                    return Redirect(Url.Action("Index", new { id = oficina.idCliente }));
                    //return RedirectToAction(nameof(Index));
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
            cargarCleinte(oficina.Cliente);
            return View(oficina);
        }

        // GET: Oficinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oficina = await _context.Oficina
                .FirstOrDefaultAsync(m => m.IdOficina == id);
            if (oficina == null)
            {
                return NotFound();
            }

            return View(oficina);
        }

        // POST: Oficinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oficina = await _context.Oficina.FindAsync(id);
            _context.Oficina.Remove(oficina);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OficinaExists(int id)
        {
            return _context.Oficina.Any(e => e.IdOficina == id);
        }
    }
}
