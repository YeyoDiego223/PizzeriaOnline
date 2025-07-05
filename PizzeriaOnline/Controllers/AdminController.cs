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
                pedido.Estado = nuevoEstado;
                _context.SaveChanges();
            }

            return RedirectToAction("DetallePedido", new { id = id });
        }

        public IActionResult DetallePedido(int id)
        {
            var pedidoConDetalles = _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefault(p => p.Id == id);

            if (pedidoConDetalles == null)
            {
                return NotFound();
            }

            return View(pedidoConDetalles);
        }       
    }
}
