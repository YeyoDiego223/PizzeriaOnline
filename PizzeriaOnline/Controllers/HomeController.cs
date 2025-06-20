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

        [HttpPost] // Este atributo indica que este método solo responde a peticiones POST
        public IActionResult AgregarAlCarrito(int pizzaId, int tamañoSeleccionadoId, int cantidad)
        {
            // 1. Obtener la pizza y el tamaño de la base de datos
            var tamaño = _context.Tamaños.Find(tamañoSeleccionadoId);
            var pizza = _context.Pizzas.Find(pizzaId);

            if (tamaño == null || pizza == null)
            {
                return NotFound(); // Si no se encuentra el producto, error.
            }

            // 2. Obtener el carrito de la sesión o crear uno nuevo si no existe
            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null
                ? new List<CarritoItem>()
                : JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);

            // 3. Revisar si el producto ya está en el carrito
            var itemExistente = carrito.FirstOrDefault(i => i.PizzaId == pizzaId && i.TamañoId == tamañoSeleccionadoId);

            if (itemExistente != null)
            {
                // Si ya existe, solo actualizamos la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si es nuevo, creamos un nuevo CarritoItem y lo añadimos a la lista
                var nuevoItem = new CarritoItem
                {
                    PizzaId = pizzaId,
                    NombrePizza = pizza.Nombre,
                    TamañoId = tamañoSeleccionadoId,
                    NombreTamaño = tamaño.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = tamaño.PrecioBase // Usamos el precio base del tamaño
                };
                carrito.Add(nuevoItem);
            }

            // 4. Guardar la lista actualizada de vuelta en la sesión
            HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(carrito));

            // 5. Redirigir al usuario de vuelta al menú
            return RedirectToAction("Index");
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
            var listaDePizzas = _context.Pizzas
                .Include(p => p.Variantes)
                    .ThenInclude(v => v.Tamaño)
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
                    .ThenInclude(v => v.Tamaño)
                .FirstOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }
    }
}