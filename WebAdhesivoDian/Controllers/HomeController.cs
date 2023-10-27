using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAdhesivoDian.Models;

namespace WebAdhesivoDian.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebAdhesivoDianContext _context;

        public HomeController(ILogger<HomeController> logger, WebAdhesivoDianContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Gestiones()
        {
            var userId = User.Identity.Name;
            Usuario usuario = _context.Usuario.Where(t => t.Nombre == userId).FirstOrDefault();
            return PartialView("Gestiones", _context.Roles.Where(t => t.Id == usuario.IdRol));
        }
    }
}
