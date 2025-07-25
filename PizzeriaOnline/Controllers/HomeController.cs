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

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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

        [Authorize]
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
        public async Task<IActionResult> FinalizarPedido(CheckoutViewModel checkoutModel)
        {
            // Coordenadas aproximadas del rectángulo que cubre tu zona de reparto.
            // Puedes ajustar estos valores para ser más preciso.
            const double minLat = 18.83; // Límite sur (Zumpahuacan)
            const double maxLat = 18.99; // Límite Norte (Tenancingo)
            const double minLng = -99.62; // Límite Oeste
            const double maxLng = -99.55; // Límite Este

            if (checkoutModel.Latitud < minLat || checkoutModel.Latitud > maxLat ||
                checkoutModel.Longitud < minLng || checkoutModel.Longitud > maxLng)
            {
                // La ubicación esta fuera de la zona. Añadimos un error al modelo.
                ModelState.AddModelError("", "Lo sentimos, tu ubicación esta fuera de nuestra zona de reparto.");
            }

            if (ModelState.IsValid)
            {
                // 1. Cargar carritos de la sesión
                var carritoJson = HttpContext.Session.GetString("Carrito");
                var carritoExtrasJson = HttpContext.Session.GetString("CarritoExtras");


                var carrito = string.IsNullOrEmpty(carritoJson)
                    ? new List<CarritoItem>()
                    : JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);

                var carritoExtras = string.IsNullOrEmpty(carritoExtrasJson)
                    ? new List<CarritoExtraViewModel>()
                    : JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJson);

                if (!carrito.Any() && !carritoExtras.Any())
                {
                    return RedirectToAction("Index"); // No hay nada que pedir
                }

                // 2. Crear el objeto Pedido principal (aún sin guardarlo)
                var nuevoPedido = new Pedido
                {
                    FechaPedido = DateTime.Now,
                    NombreCliente = checkoutModel.NombreCliente,
                    DireccionEntrega = checkoutModel.DireccionEntrega,
                    Telefono = checkoutModel.Telefono,
                    Estado = "Recibido",
                    Latitud = checkoutModel.Latitud,
                    Longitud = checkoutModel.Longitud
                    // El TotalPedido lo calcularemos al final
                };

                decimal totalCalculado = 0;

                // 3. Crear los Detalles del Pedido para las PIZZAS
                foreach (var item in carrito)
                {
                    var nuevoDetalle = new DetallePedido
                    {

                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioFinal,
                        NombreTamaño = item.NombreTamaño,
                        TamañoId = item.TamañoId,
                        DetalleSabores = new List<DetalleSabor>(),
                        PedidoId = nuevoPedido.Id

                    };
                    // --- INICIO DEL CÓDIGO QUE FALTA ---

                    // Buscamos las pizzas (sabores) que corresponden a este item del carrito
                    var pizzasDeEsteItem = await _context.Pizzas
                                                           .Where(p => item.NombresSabores.Contains(p.Nombre))
                                                           .ToListAsync();

                    // Creamos la conexión en la tabla puente DetalleSabor
                    foreach (var pizzaSabor in pizzasDeEsteItem)
                    {
                        nuevoDetalle.DetalleSabores.Add(new DetalleSabor
                        {
                            PizzaId = pizzaSabor.Id
                        });
                    }
                    // --- FIN DEL CÓDIGO QUE FALTA ---

                    nuevoPedido.Detalles.Add(nuevoDetalle);
                    totalCalculado += nuevoDetalle.PrecioUnitario * nuevoDetalle.Cantidad;
                }

                // 4. Crear los Detalles del Pedido para los EXTRAS
                foreach (var extra in carritoExtras)
                {
                    var nuevoDetalleExtra = new DetallePedido
                    {
                        Cantidad = extra.Cantidad,
                        PrecioUnitario = extra.PrecioUnitario,
                        PedidoId = nuevoPedido.Id,
                        NombreTamaño = extra.Nombre // <-- AÑADE ESTA LÍNEA
                    };
                    nuevoPedido.Detalles.Add(nuevoDetalleExtra);

                }

                // 5. Asignar el total final y guardar TODO de una sola vez
                nuevoPedido.TotalPedido = totalCalculado;
                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync(); // <-- UN ÚNICO GUARDADO ASÍNCRONO

                // 6. Limpiar sesión y redirigir
                HttpContext.Session.Remove("Carrito");
                HttpContext.Session.Remove("CarritoExtras");
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, recargamos los datos para la vista
            var carritoJsonInvalido = HttpContext.Session.GetString("Carrito");
            if (!string.IsNullOrEmpty(carritoJsonInvalido))
            {
                checkoutModel.Carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJsonInvalido);
                checkoutModel.TotalCarrito = checkoutModel.Carrito.Sum(i => i.PrecioFinal * i.Cantidad);
            }

            // Reutilizamos la misma variable de arriba, no la volvemos a declarar con 'var'
            var carritoExtrasJsonInvalido = HttpContext.Session.GetString("CarritoExtras");
            if (!string.IsNullOrEmpty(carritoExtrasJsonInvalido))
            {
                checkoutModel.CarritoExtras = JsonConvert.DeserializeObject<List<CarritoExtraViewModel>>(carritoExtrasJsonInvalido);
            }

            checkoutModel.ExtrasDisponibles = _context.ProductoExtras.Where(p => p.CantidadEnStock > 0).ToList();
            return View("Checkout", checkoutModel);
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