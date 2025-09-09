// ---- INICIO DEL CÓDIGO CORRECTO Y LIMPIO ----

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using PizzeriaOnline.ViewModels;
using PizzeriaOnline.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Services.TiendaService _tiendaService;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<HomeController> logger, IConfiguration configuration, Services.TiendaService tiendaService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _tiendaService = tiendaService;
        }

        public IActionResult Index()
        {
            var configuracion = _context.Configuracion.FirstOrDefault(); // Obtenemos toda la configuración

            var listaDePizzas = _context.Pizzas
                .Include(p => p.Imagenes)
                .ToList();

            var viewModel = new HomeIndexViewModel
            {
                PizzasDisponibles = listaDePizzas,
                Configuracion = configuracion
            };

            bool estaAbiertaResultado = _tiendaService.EstaAbierta();
            ViewData["EstaAbierta"] = _tiendaService.EstaAbierta();
            return View(viewModel);            
        }

        public IActionResult Constructor()
        {
            var viewModel = new ConstructorViewModel
            {
                // Primero trae los datos con ToList(), LUEGO ordena con OrderBy()
                TamañosDisponibles = _context.Tamaños.ToList().OrderBy(t => t.PrecioBase).ToList(),
                SaboresDisponibles = _context.Pizzas
                                        .Include(p => p.Imagenes)
                                        .OrderBy(p => p.Nombre)
                                        .ToList()
            };
            ViewData["EstaAbierta"] = _tiendaService.EstaAbierta();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Cambiamos el parámetro para que acepte los datos directamente del formulario
        public IActionResult AgregarPizzaPersonalizada(int tamañoId, List<int> saboresIds)
        {
            // 1. Validamos que recibimos los datos necesarios
            if (tamañoId <= 0 || saboresIds == null || !saboresIds.Any())
            {
                // Si faltan datos, redirigimos de vuelta al constructor
                return RedirectToAction("Constructor");
            }

            // 2. Obtenemos el objeto 'Tamaño' desde la base de datos
            var tamaño = _context.Tamaños.Find(tamañoId);
            if (tamaño == null)
            {
                // Si el tamaño no existe, no podemos continuar
                return RedirectToAction("Constructor");
            }

            // 3. Obtenemos los nombres de los sabores seleccionados
            var nombresSabores = _context.Pizzas
                                        .Where(p => saboresIds.Contains(p.Id))
                                        .Select(p => p.Nombre)
                                        .ToList();

            // 4. Creamos el nuevo item para el carrito
            var nuevoItem = new CarritoItem
            {
                // Se genera un nuevo ID único para este item en el carrito
                Id = Guid.NewGuid(),
                TamañoId = tamaño.Id,
                NombreTamaño = tamaño.Nombre,
                // El precio final es el precio base del tamaño
                PrecioFinal = tamaño.PrecioBase,
                // Agregamos la lista de nombres de sabores que consultamos
                NombresSabores = nombresSabores,
                Cantidad = 1
            };

            // 5. Obtenemos el carrito actual de la sesión (o creamos uno nuevo si no existe)
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carrito = string.IsNullOrEmpty(carritoJson)
                ? new List<CarritoItem>()
                : JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);

            // 6. Agregamos el nuevo item y guardamos el carrito de vuelta en la sesión
            carrito.Add(nuevoItem);
            HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(carrito));

            // 7. Redirigimos al usuario a la vista del carrito para que vea su pizza
            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            var pizzas = !string.IsNullOrEmpty(carritoJson) ? JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson) : new List<CarritoItem>();
            var extras = !string.IsNullOrEmpty(carritoExtrasJson) ? JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson) : new List<CarritoExtraViewModel>();

            var viewModel = new CarritoViewModel
            {
                Pizzas = pizzas,
                Extras = extras,
                TotalGeneral = pizzas.Sum(p => p.PrecioFinal * p.Cantidad) + extras.Sum(e => e.Subtotal),
                ExtrasDisponibles = await _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToListAsync()
            };
            ViewData["EstaAbierta"] = _tiendaService.EstaAbierta();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult QuitarPizza(Guid id)
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            if (!string.IsNullOrEmpty(carritoJson))
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

        [HttpPost]
        public IActionResult AgregarExtra(int productoExtraId, int cantidad = 1)
        {
            var producto = _context.ProductoExtras.Find(productoExtraId);
            if (producto == null) return RedirectToAction("Carrito");

            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            var carritoExtras = string.IsNullOrEmpty(carritoExtrasJson) ? new List<CarritoExtraViewModel>() : JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);

            var itemExistente = carritoExtras.FirstOrDefault(i => i.ProductoExtraId == productoExtraId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carritoExtras.Add(new CarritoExtraViewModel
                {
                    ProductoExtraId = producto.Id,
                    Nombre = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            HttpContext.Session.SetString("CarritoExtras", JsonConvert.SerializeObject(carritoExtras));
            return RedirectToAction("Carrito");
        }

        [HttpPost]
        public IActionResult QuitarExtra(int productoExtraId)
        {
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            if (!string.IsNullOrEmpty(carritoExtrasJson))
            {
                var carritoExtras = JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);
                var itemParaQuitar = carritoExtras.FirstOrDefault(i => i.ProductoExtraId == productoExtraId);
                if (itemParaQuitar != null)
                {
                    carritoExtras.Remove(itemParaQuitar);
                    HttpContext.Session.SetString("CarritoExtras", JsonConvert.SerializeObject(carritoExtras));
                }
            }
            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Checkout()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");

            // --- AÑADE ESTA VALIDACIÓN AL INICIO ---
            if (string.IsNullOrEmpty(carritoJson) && string.IsNullOrEmpty(carritoExtrasJson))
            {
                // Si ambos carritos están vacíos, no hay nada que finalizar.
                // Redirigimos al usuario a la página del carrito, donde verá un mensaje claro.
                return RedirectToAction("Carrito");
            }
            // --- FIN DE LA VALIDACIÓN ---

            var googleApiKey = _configuration["Google:ApiKey"];
            ViewData["GoogleApiKey"] = googleApiKey;

            var carrito = !string.IsNullOrEmpty(carritoJson)
                ? JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson)
                : new List<CarritoItem>();

            var carritoExtras = !string.IsNullOrEmpty(carritoExtrasJson)
                ? JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson)
                : new List<CarritoExtraViewModel>();

            var viewModel = new CheckoutViewModel
            {
                Carrito = carrito,
                CarritoExtras = carritoExtras,
                TotalCarrito = carrito.Sum(item => item.PrecioFinal * item.Cantidad) + carritoExtras.Sum(e => e.Subtotal),
                ExtrasDisponibles = await _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Contacto()
        {
            return View();
        }                      

        public IActionResult PedidoConfirmado(int id)
        {
            ViewBag.PedidoId = id;
            return View();
        }      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarPedido(CheckoutViewModel checkoutModel)
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");
            var carrito = !string.IsNullOrEmpty(carritoJson) ? JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson) : new List<CarritoItem>();
            var carritoExtras = !string.IsNullOrEmpty(carritoExtrasJson) ? JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson) : new List<CarritoExtraViewModel>();

            ValidarPedido(checkoutModel, carrito, carritoExtras);

            if (!ModelState.IsValid)
            {
                checkoutModel.Carrito = carrito;
                checkoutModel.CarritoExtras = carritoExtras;
                checkoutModel.TotalCarrito = carrito.Sum(i => i.PrecioFinal * i.Cantidad) + carritoExtras.Sum(e => e.Subtotal);
                checkoutModel.ExtrasDisponibles = await _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToListAsync();
                return View("Checkout", checkoutModel);
            }

            var nuevoPedido = await CrearPedidoDesdeSesion(checkoutModel, carrito, carritoExtras);

            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Carrito");
            HttpContext.Session.Remove("CarritoExtras");

            return RedirectToAction("VerPedido", new { token = nuevoPedido.AccesoToken });
        }

        // --- MÉTODOS PRIVADOS AUXILIARES ---

        private void ValidarPedido(CheckoutViewModel checkoutModel, List<CarritoItem> carrito, List<CarritoExtraViewModel> carritoExtras)
        {
            if (!carrito.Any() && !carritoExtras.Any())
            {
                ModelState.AddModelError("", "Tu carrito está vacío. No puedes finalizar el pedido.");
            }

            const double minLat = 18.83, maxLat = 18.99, minLng = -99.62, maxLng = -99.55;
            if (checkoutModel.Latitud < minLat || checkoutModel.Latitud > maxLat || checkoutModel.Longitud < minLng || checkoutModel.Longitud > maxLng)
            {
                ModelState.AddModelError("DireccionEntrega", "Lo sentimos, tu ubicación está fuera de nuestra zona de reparto.");
            }
        }

        private async Task<Pedido> CrearPedidoDesdeSesion(CheckoutViewModel checkoutModel, List<CarritoItem> carrito, List<CarritoExtraViewModel> carritoExtras)
        {
            var nuevoPedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                NombreCliente = checkoutModel.NombreCliente,
                DireccionEntrega = checkoutModel.DireccionEntrega,
                Telefono = checkoutModel.Telefono,
                Estado = "Recibido",
                MetodoPago = checkoutModel.MetodoPago,
                Latitud = checkoutModel.Latitud,
                Longitud = checkoutModel.Longitud,
                Detalles = new List<DetallePedido>(),
                AccesoToken = Guid.NewGuid()
            };


            decimal totalCalculado = 0;

            foreach (var item in carrito)
            {
                var detallePizza = new DetallePedido
                {
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioFinal,
                    NombreTamaño = item.NombreTamaño,
                    TamañoId = item.TamañoId,
                    DetalleSabores = new List<DetalleSabor>()
                };

                var pizzasDeEsteItem = await _context.Pizzas
                    .Where(p => item.NombresSabores.Contains(p.Nombre))
                    .ToListAsync();

                foreach (var pizzaSabor in pizzasDeEsteItem)
                {
                    detallePizza.DetalleSabores.Add(new DetalleSabor { PizzaId = pizzaSabor.Id });
                }

                nuevoPedido.Detalles.Add(detallePizza);
                totalCalculado += detallePizza.PrecioUnitario * detallePizza.Cantidad;
            }

            foreach (var extra in carritoExtras)
            {
                var detalleExtra = new DetallePedido
                {
                    Cantidad = extra.Cantidad,
                    PrecioUnitario = extra.PrecioUnitario,
                    NombreTamaño = extra.Nombre,
                    ProductoExtraId = extra.ProductoExtraId
                };
                nuevoPedido.Detalles.Add(detalleExtra);
                totalCalculado += detalleExtra.PrecioUnitario * extra.Cantidad;
            }

            nuevoPedido.TotalPedido = totalCalculado;
            return nuevoPedido;
        }

        public async Task<IActionResult> VerPedido(Guid token)
        {
            if (token == Guid.Empty)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.AccesoToken == token);

            if (pedido == null)
            {
                return NotFound();
            }

            var googleApiKey = _configuration["Google:ApiKey"];
            ViewData["GoogleApiKey"] = googleApiKey;

            return View("PedidoConfirmado", pedido);
        }

        public IActionResult MisPedidos()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDetallesPedido(Guid token)
        {
            if (token == Guid.Empty)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.AccesoToken == token);

            if (pedido == null)
            {
                return Json(new { error = "Pedido no encontrado " });
            }

            return Json(new
            {
                id = pedido.Id,
                fecha = pedido.FechaPedido,
                estado = pedido.Estado,
                token = pedido.AccesoToken
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarDatosPedido(int id, string nombreCliente, string telefono, string direccionEntrega, double latitud, double longitud)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            // Doble checo de seguridad: solo se puede modificar si el estado es "Recibido"
            if (pedido.Estado != "Recibido")
            {
                return Forbid("Este pedido ya no se puede modificar.");
            }

            pedido.NombreCliente = nombreCliente;
            pedido.Telefono = telefono;
            pedido.DireccionEntrega = direccionEntrega;
            pedido.Latitud = latitud;
            pedido.Longitud = longitud;

            await _context.SaveChangesAsync();

            return RedirectToAction("VerPedido", new { token = pedido.AccesoToken });
        }
    }
}