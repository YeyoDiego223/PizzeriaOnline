using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using Microsoft.Extensions.FileProviders;

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var pizza = _context.Pizzas.Find(id);
            if(pizza != null)
            {
                _context.Pizzas.Remove(pizza);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var pizza = _context.Pizzas.Find(id);
            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }
        public IActionResult Edit(int id)
        {
            var pizza = _context.Pizzas.Find(id);

            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pizza pizza)
        {
            if(ModelState.IsValid)
            {
                _context.Update(pizza);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {

            if (ModelState.IsValid)
            {
                _context.Add(pizza);
                _context.SaveChanges();
            }          
            return RedirectToAction("I");
        }

        public IActionResult Index()
        {
            var listadePizzas = _context.Pizzas.ToList();
            return View(listadePizzas);
        }
    }
}
