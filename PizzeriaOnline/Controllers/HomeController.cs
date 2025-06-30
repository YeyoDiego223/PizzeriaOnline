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

namespace PizzeriaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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
                TotalCarrito = carrito.Sum(item => item.PrecioFinal * item.Cantidad)
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Privacy()
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
        public IActionResult FinalizarPedido(CheckoutViewModel checkoutModel)
        {
            if (ModelState.IsValid)
            {
                var carritoJson = HttpContext.Session.GetString("Carrito");   
                if (string.IsNullOrEmpty(carritoJson))
                {
                    return RedirectToAction("Index");
                }
                var carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJson);
                if (carrito == null || !carrito.Any())
                {
                    return RedirectToAction("Index");
                }

                var nuevoPedido = new Pedido
                {
                    FechaPedido = DateTime.Now,
                    NombreCliente = checkoutModel.NombreCliente,
                    DireccionEntrega = checkoutModel.DireccionEntrega,
                    Telefono = checkoutModel.Telefono,
                    TotalPedido = carrito.Sum(c => c.PrecioFinal * c.Cantidad),
                    Estado = "Recibido"
                };

                foreach (var item in carrito)
                {
                    var saboresTexto = string.Join(" / ", item.NombresSabores);
                    var descripcionCompleta = $"{item.NombreTamaño} ({saboresTexto})";

                    var nuevoDetalle = new DetallePedido
                    {
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioFinal,
                        DescripcionProducto = descripcionCompleta
                    };
                    nuevoPedido.Detalles.Add(nuevoDetalle);
                }
                _context.Pedidos.Add(nuevoPedido);
                _context.SaveChanges();
                HttpContext.Session.Remove("Carrito");

                return View("Checkout", checkoutModel);
            }

            var carritoJsonInvalido = HttpContext.Session.GetString("Carrito");
            if(!string.IsNullOrEmpty(carritoJsonInvalido))
            {
                checkoutModel.Carrito = JsonConvert.DeserializeObject<List<CarritoItem>>(carritoJsonInvalido);
                checkoutModel.TotalCarrito = checkoutModel.Carrito.Sum(i => i.PrecioFinal * i.Cantidad);
            }
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