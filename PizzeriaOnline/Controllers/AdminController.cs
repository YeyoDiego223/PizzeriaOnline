using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using PizzeriaOnline.ViewModels;
using System.Drawing.Printing;



namespace PizzeriaOnline.Controllers
{
    

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
         public IActionResult Index(string terminoBusqueda)
        {            
            // 1. Empezamos con una consulta "base" a la tabla de Pedidos.
            //      Ojo: Aún no hemos ejecutado la consulta (no hay .ToList()).
            var pedidosQuery = _context.Pedidos.AsQueryable();

            // Guardamos el termino de búsqueda para poder mostrarlo en la vista después
            ViewData["BusqueadaActual"] = terminoBusqueda;

            // 2. Si el usuario escribió algo en la caja de búsqueda...
            if (!string.IsNullOrEmpty(terminoBusqueda))
            {
                // 3. ...apilcamos un filtro a nuestra consulta base.
                //    Buscamos en el Nombre del Cliente 0 si el ID del pedido coincide.
                pedidosQuery = pedidosQuery.Where(p => p.NombreCliente.Contains(terminoBusqueda)
                                                    || p.Id.ToString() == terminoBusqueda);
            }
         
            // 4. Ahora sí, ordenamos y ejecutamos la consulta final (sea la original o la filtrada)
            var listaDePedidos = pedidosQuery.OrderByDescending(p => p.FechaPedido).ToList();

            // Obtener la fecha de hoy, sin la hora.
            var hoy = DateTime.Today;

            // Calculamos el total de ventas, sumando solo los pedidos que están 'Completado'.
            var totalVentasHoy = _context.Pedidos
                .Where(p => p.Estado == "Completado" && p.FechaPedido.Date == hoy)
                .Sum(p => p.TotalPedido);

            // Guardamos el resultado en ViewData para poder usarlo en la vista.
            ViewData["TotalVentasHoy"] = totalVentasHoy;
            

            // Contamos cuantos pedidos tienen una FechaPedido mayo o igual a la de hoy.
            var pedidosHoy = _context.Pedidos
                .Count(p => p.FechaPedido.Date == hoy);

            // Guardamos el resultado en ViewData
            ViewData["PedidosHoy"] = pedidosHoy;

            // Definimos un umbral para considerar el stock como bajo.
            const decimal umbralBajo = 5.0m;

            // Buscamos los ingredientes que están por debajo de ese umbral.
            var ingredientesBajos = _context.Ingredientes
                .Where(i => i.CantidadEnStock < umbralBajo)
                .ToList();

            // Guardamos la lista completa en ViewData.
            ViewData["IngredientesBajos"] = ingredientesBajos;

            var pizzasPopulares = _context.DetalleSabores
                .Include(ds => ds.Pizza)
                .GroupBy(ds => ds.Pizza.Nombre) // Agrupamos por el nombre de la pizza
                .Select(g => new PizzaPopularViewModel {
                    NombrePizza = g.Key,
                    CantidadVendida = g.Count()
                })
                .OrderByDescending(p => p.CantidadVendida)  // Ordenamos de la más vendida a la menos
                .Take(3)                        // Tomamos solo los 3 primeros resultados
                .ToList();

            
            ViewData["PizzasPopulares"] = pizzasPopulares;

            return View(listaDePedidos);
        }
       

        [HttpPost]
        public IActionResult ActualizarEstado(int id, string nuevoEstado)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido != null && !string.IsNullOrEmpty(nuevoEstado))
            {
                if (nuevoEstado == "En preparación" && pedido.Estado != "En preparación")
                {
                    // Cargamos los detalles del pedido con toda la información necesaria
                    var detallesDelPedido = _context.DetallePedidos
                                                    .Where(d => d.PedidoId == id)
                                                    .Include(d => d.DetalleSabores)
                                                    .ToList();

                    foreach (var detalle in detallesDelPedido)
                    {
                        // Por cada pizza personalizada en el pedido...
                        foreach (var sabor in detalle.DetalleSabores)
                        {
                            // Buscamos la receta para esta pizza Y este tamaño específico
                            var receta = _context.Recetas
                                .FirstOrDefault(r => r.PizzaId == sabor.PizzaId && r.TamañoId == detalle.TamañoId);

                            if (receta != null)
                            {
                                var ingredienteEnStock = _context.Ingredientes.Find(receta.IngredienteId);
                                if (ingredienteEnStock != null)
                                {
                                    ingredienteEnStock.CantidadEnStock -= receta.Cantidad * detalle.Cantidad;
                                }
                            }
                        }
                    }
                }
                pedido.Estado = nuevoEstado;
                _context.SaveChanges();
            }

            return RedirectToAction("DetallePedido", new { id = id });
        }

        public IActionResult DetallePedido(int id)
        {
           var pedidoConDetalles = _context.Pedidos
                // 1. Incluye la lista de Detalles del Pedido
                .Include(p => p.Detalles)
                // 2. Y LUEGO, por cada Detalle, incluye su lista de Sabores
                .ThenInclude(d => d.DetalleSabores)
                // 3. Y LUEGO, por cada Sabor, incluye la información de la Pizza (para tener el nombre)
                .ThenInclude(ds => ds.Pizza)
                .FirstOrDefault(p => p.Id == id);
            if (pedidoConDetalles == null)
            {
                return NotFound();
            }

            return View(pedidoConDetalles);
        }       
    }
}
