using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;

namespace PizzeriaOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listaDeIngredientes = _context.Ingredientes.ToList();
            return View(listaDeIngredientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ingrediente ingrediente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingrediente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ingrediente);
        }
        public IActionResult Create()
        {

            return View();
        }
    }
}
