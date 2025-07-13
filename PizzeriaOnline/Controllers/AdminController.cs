using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
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

            return View(listaDePedidos);
        }
       

        [HttpPost]
        public IActionResult ActualizarEstado(int id, string nuevoEstado)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido != null && !string.IsNullOrEmpty(nuevoEstado))
            {
                // Verificamos si el estado cambiado A "En preparación"
                // y que no lo estuviera ya antes, para evitar descontar el stock dos veces.
                if (nuevoEstado == "En preparación" && pedido.Estado != "En preparación")
                {
                    // --- INICIO DE LA LÓGICA DE DESCUENTO DE INVENTARIO ---

                    // 1. Cargamos los detalles y sabores de este pedido especifico.
                    var detallesDelPedido = _context.DetallePedidos
                        .Include(d => d.DetalleSabores)
                        .Where(d => d.PedidoId == id)
                        .ToList();

                    foreach (var detalle in detallesDelPedido)
                    {
                            // Por cada pizza personalizada en el pedido...
                            foreach (var sabor in detalle.DetalleSabores)
                            {
                                // ...buscamos su receta (la lista de ingredientes y cantidades que necesita).
                            var receta = _context.PizzaIngredientes
                                .Where(pi => pi.PizzaId == sabor.PizzaId)
                                .ToList();

                            foreach (var recetaIngrediente in receta)
                            {
                                // Por cada ingrediente en la receta...
                                var ingredienteEnStock = _context.Ingredientes.Find(recetaIngrediente.IngredienteId);
                                if (ingredienteEnStock != null)
                                {
                                    // ...!descontamos la cantidad del stock!
                                    ingredienteEnStock.CantidadEnStock -= recetaIngrediente.Cantidad;
                                }
                            }
                        }
                    }
                }
                // --- FIN DE LA LÓGICA DE DESCUENTO ---
                pedido.Estado = nuevoEstado;
                // Guardamos TODOS los cambios (el nuevo estado del pedido Y las nuevas cantidades de los ingredientes)
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
