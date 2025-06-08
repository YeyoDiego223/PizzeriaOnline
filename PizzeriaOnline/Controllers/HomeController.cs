// Aseg�rate de tener los usings necesarios al principio del archivo
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Importante para el m�todo .Include()
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Diagnostics;
using System.Linq;

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        // --- PASO 1 (Declaraci�n del campo) ---
        // Aqu� declaramos una variable _context a nivel de la clase.
        // Es 'private' porque solo se usar� dentro de esta clase.
        // Es 'readonly' porque solo le asignaremos un valor una vez, en el constructor.
        private readonly ApplicationDbContext _context;

        // --- PASO 2 (Asignaci�n en el Constructor) ---
        // Este es el constructor de la clase. Se ejecuta cuando se crea un HomeController.
        // Recibe una instancia de ApplicationDbContext (gracias a la Inyecci�n de Dependencias que configuramos en Program.cs)
        // y la asigna a nuestra variable _context.
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- PASO 3 (Uso del campo) ---
        // Ahora que _context existe y tiene un valor, el m�todo Index puede usarlo.
        public IActionResult Index()
        {
            var listaDePizzas = _context.Pizzas
                .Include(p => p.Variantes)
                    .ThenInclude(v => v.Tama�o)
                .ToList();

            return View(listaDePizzas);
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

        public IActionResult Detalle(int id)
        {
            var pizza = _context.Pizzas
                .Include(p => p.Variantes)
                    .ThenInclude(v => v.Tama�o)
                .FirstOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }
    }
}