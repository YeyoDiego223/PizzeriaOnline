using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaOnline.Data;



namespace PizzeriaOnline.Controllers
{
    

    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listaDePedidos = _context.Pedidos.OrderByDescending(p => p.FechaPedido).ToList();
            return View(listaDePedidos);
        }
    }
}
