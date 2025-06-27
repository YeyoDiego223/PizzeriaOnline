using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;

namespace PizzeriaOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PizzasAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PizzasAdminController(ApplicationDbContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listadePizzas = _context.Pizzas.ToList();
            return View(listadePizzas);
        }
    }
}
