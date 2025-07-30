// ---- INICIO DEL CÓDIGO CORRECTO Y LIMPIO ----

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PizzeriaOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;
using PizzeriaOnline.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult QuitarExtra(int productoExtraId)
        {
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            if (string.IsNullOrEmpty(carritoExtrasJson))
            {
                return RedirectToAction("Checkout");
            }

            var carritoExtras = JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);

            // Buscamos el item a quitar en la lista.
            var itemParaQuitar = carritoExtras.FirstOrDefault(i => i.ProductoExtraId == productoExtraId);

            if (itemParaQuitar != null)
            {
                // Removemos de la lista.
                carritoExtras.Remove(itemParaQuitar);
                // Guardamos la lista modificada de vuelta en la session.
                HttpContext.Session.SetString("CarritoExtras", JsonConvert.SerializeObject(carritoExtras));
            }

            // Redirigimos de vuelta a la página de checkout para que vea el cambio..
            return RedirectToAction("Checkout");
        }

        [HttpPost]
        public IActionResult AgregarExtra(int productoExtraId, int cantidad)
        {
            // Buscamos el producto en la BD para asegurarnos de que existe y tener su precio.
            var producto = _context.ProductoExtras.Find(productoExtraId);
            if (producto == null || cantidad <= 0)
            {
                // Si hay un error, simplemente volvemos al checkout.
                return RedirectToAction("Checkout");
            }

            // Obtenemos el carrito de extras de la sesión o creamos uno nuevo.
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            List<CarritoExtraViewModel> carritoExtras = carritoExtrasJson == null
                ? new List<CarritoExtraViewModel>()
                : JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);

            // Verificamos si el producto ya esta en el carrito de extras.
            var itemExistente = carritoExtras.FirstOrDefault(i => i.ProductoExtraId == productoExtraId);
            if (itemExistente != null)
            {
                // Si ya existe, solo aumentamos la cantidad.
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si es nuevo, lo añadimos a la lista.
                carritoExtras.Add(new CarritoExtraViewModel
                {
                    ProductoExtraId = producto.Id,
                    Nombre = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            // Guardamos la Lista actualizada de vuelta en la sesión.
            HttpContext.Session.SetString("CarritoExtras", JsonConvert.SerializeObject(carritoExtras));

            // Redirigimos de vuelta a la página de checkout
            return RedirectToAction("Checkout");
        }

        public IActionResult Checkout()
        {           
            var carritoJson = HttpContext.Session.GetString("Carrito");
            if (string.IsNullOrEmpty(carritoJson))
            {
                // Si no hay carrito, no se puede hacer checkout. Lo mandamos al carrito vacío.
                return RedirectToAction("Carrito");
            }

            var carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);

            // Creamos el ViewModel y lo llenamos con los datos del carrito
            var viewModel = new CheckoutViewModel
            {
                Carrito = carrito,
                TotalCarrito = carrito.Sum(item => item.PrecioFinal * item.Cantidad),
                ExtrasDisponibles = _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToList(),
            };

            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            if (!string.IsNullOrEmpty(carritoExtrasJson))
            {
                viewModel.CarritoExtras = JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);
            }

            return View(viewModel);
        }

        public IActionResult Contacto()
        {
            return View();
        }

        public IActionResult Index()
        {
            var listaDePizzas = _context.Pizzas.ToList();
            return View(listaDePizzas);
        }

        public IActionResult Constructor()
        {
            var viewModel = new ConstructorViewModel
            {
                TamañosDisponibles = _context.Tamaños.ToList().OrderBy(t => t.PrecioBase).ToList(),
                SaboresDisponibles = _context.Pizzas.OrderBy(p => p.Nombre).ToList()
            };
            return View(viewModel);
        }

        public IActionResult Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito;
            if (carritoJson == null)
            {
                carrito = new List<CarritoItem>();
            }
            else
            {
                carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);
            }
            return View(carrito);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarPedido(CheckoutViewModel checkoutModel)
        {
            // Recargamos los carritos desde la sesión al principio.
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            var carrito = !string.IsNullOrEmpty(carritoJson) ? JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson) : new List<CarritoItem>();
            var carritoExtras = !string.IsNullOrEmpty(carritoExtrasJson) ? JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson) : new List<CarritoExtraViewModel>();

            if (!carrito.Any() && !carritoExtras.Any())
            {
                ModelState.AddModelError("", "Tu carrito está vacío.");
            }

            // Validación de zona de reparto.
            const double minLat = 18.83, maxLat = 18.99, minLng = -99.62, maxLng = -99.55;
            if (checkoutModel.Latitud < minLat || checkoutModel.Latitud > maxLat || checkoutModel.Longitud < minLng || checkoutModel.Longitud > maxLng)
            {
                ModelState.AddModelError("", "Lo sentimos, tu ubicación está fuera de nuestra zona de reparto.");
            }

            if (!ModelState.IsValid)
            {
                // Si hay cualquier error, reconstruimos el ViewModel y volvemos a la vista.
                checkoutModel.Carrito = carrito;
                checkoutModel.CarritoExtras = carritoExtras;
                checkoutModel.TotalCarrito = carrito.Sum(i => i.PrecioFinal * i.Cantidad) + carritoExtras.Sum(e => e.Subtotal);
                checkoutModel.ExtrasDisponibles = await _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToListAsync();
                return View("Checkout", checkoutModel);
            }

            // --- SI TODO ES VÁLIDO, PROCEDEMOS A CREAR EL PEDIDO ---

            var nuevoPedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                NombreCliente = checkoutModel.NombreCliente,
                DireccionEntrega = checkoutModel.DireccionEntrega,
                Telefono = checkoutModel.Telefono,
                Estado = "Recibido",
                Latitud = checkoutModel.Latitud,
                Longitud = checkoutModel.Longitud,
                Detalles = new List<DetallePedido>()
            };

            // Procesar Pizzas
            foreach (var item in carrito)
            {
                var nuevoDetalle = new DetallePedido
                {
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioFinal,
                    NombreTamaño = item.NombreTamaño,
                    TamañoId = item.TamañoId,
                };

                var pizzasDeEsteItem = await _context.Pizzas
                    .Where(p => item.NombresSabores.Contains(p.Nombre))
                    .ToListAsync();

                foreach (var pizzaSabor in pizzasDeEsteItem)
                {
                    nuevoDetalle.DetalleSabores.Add(new DetalleSabor { PizzaId = pizzaSabor.Id });
                }
                nuevoPedido.Detalles.Add(nuevoDetalle);

            }

            // Procesar Extras y descontar su stock
            foreach (var extra in carritoExtras)
            {
                nuevoPedido.Detalles.Add(new DetallePedido
                {
                    Cantidad = extra.Cantidad,
                    PrecioUnitario = extra.PrecioUnitario,
                    NombreTamaño = extra.Nombre
                });

                var productoEnStock = await _context.ProductoExtras.FindAsync(extra.ProductoExtraId);
                if (productoEnStock != null)
                {
                    productoEnStock.CantidadEnStock -= extra.Cantidad;
                }
            }

            nuevoPedido.TotalPedido = nuevoPedido.Detalles.Sum(d => d.PrecioUnitario * d.Cantidad);

            // Cambiamos el estado inicial a "Pendiente de Pago"
            nuevoPedido.Estado = "Pendiente de Pago";
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            // Limpiamos los carritos
            HttpContext.Session.Remove("Carrito");
            HttpContext.Session.Remove("CarritoExtras");

            // Redirigimos al controlador de pago con el ID del pedido recién creado
            return RedirectToAction("CrearCheckoutSession", "Pago", new { pedidoId = nuevoPedido.Id });
        }

        public IActionResult PedidoConfirmado(int id)
        {
            ViewBag.PedidoId = id;
            return View();
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito(int tamañoId, List<int> saboresIds)
        {
            if (saboresIds == null || !saboresIds.Any())
            {
                return RedirectToAction("Constructor");
            }
            var tamañoSeleccionado = _context.Tamaños.Find(tamañoId);
            var saboresSeleccionados = _context.Pizzas.Where(p => saboresIds.Contains(p.Id)).ToList();
            if (tamañoSeleccionado == null || !saboresSeleccionados.Any())
            {
                return NotFound();
            }

            var nuevoItem = new CarritoItem
            {
                TamañoId = tamañoId,
                NombreTamaño = tamañoSeleccionado.Nombre,
                PrecioFinal = tamañoSeleccionado.PrecioBase,
                NombresSabores = saboresSeleccionados.Select(p => p.Nombre).ToList(),
                Cantidad = 1
            };

            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null
                ? new List<CarritoItem>()
                : JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);

            carrito.Add(nuevoItem);
            HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(carrito));

            return RedirectToAction("Carrito");
        }

        [HttpPost]
        public IActionResult QuitarDelCarrito(Guid id)
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");

            if (carritoJson != null)
            {
                var carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);
                var itemParaQuitar = carrito.FirstOrDefault(i => i.Id == id);

                if (itemParaQuitar != null)
                {
                    carrito.Remove(itemParaQuitar);
                    HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(carrito));
                }
            }

            return RedirectToAction("Carrito");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

// ---- FIN DEL CÓDIGO ----