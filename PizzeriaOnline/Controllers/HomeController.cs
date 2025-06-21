// Asegúrate de tener los usings necesarios al principio del archivo
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Importante para el método .Include()
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SQLitePCL;
using PizzeriaOnline.ViewModels;
using NuGet.Protocol;

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");

            List<CarritoItem> carrito;

            if (carritoJson == null)
            {
                // Si no hay nada en la sesión, el carrito es una nueva lista vacía
                carrito = new List<CarritoItem>();
            }
            else
            {
                // Si hay algo, lo convertimos de JSON a nuestra lista de objetos
                carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);
            }

            // Pasamos la lista (llena o vacía) a la nueva vista que vamos a crear 
            return View(carrito);
        }


        // --- PASO 1 (Declaración del campo) ---
        // Aquí declaramos una variable _context a nivel de la clase.
        // Es 'private' porque solo se usará dentro de esta clase.
        // Es 'readonly' porque solo le asignaremos un valor una vez, en el constructor.
        private readonly ApplicationDbContext _context;

        // --- PASO 2 (Asignación en el Constructor) ---
        // Este es el constructor de la clase. Se ejecuta cuando se crea un HomeController.
        // Recibe una instancia de ApplicationDbContext (gracias a la Inyección de Dependencias que configuramos en Program.cs)
        // y la asigna a nuestra variable _context.
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- PASO 3 (Uso del campo) ---
        // Ahora que _context existe y tiene un valor, el método Index puede usarlo.
        public IActionResult Index()
        {
            var listaDePizzas = _context.Pizzas.ToList();

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

        public IActionResult Constructor()
        {
            // 1. Creamos una instancia de nuestro nuevo ViewModel.
            var viewModel = new ConstructorViewModel();

            // 2. Llenamos las listas del ViewModel con los datos de la base de datos.
            //    Usamos OrderBy para que se muestren en un orden lógico.
            viewModel.TamañosDisponibles = _context.Tamaños.ToList().OrderBy(t => t.PrecioBase).ToList();
            viewModel.SaboresDisponibles = _context.Pizzas.OrderBy(p => p.Nombre).ToList();

            // 3. Pasamos el ViewModel (que contiene ambas listas) a la Vista.
            return View(viewModel);
        }

    }
}