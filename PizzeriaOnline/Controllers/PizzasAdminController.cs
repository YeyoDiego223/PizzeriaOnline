using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using Microsoft.Extensions.FileProviders;
using PizzeriaOnline.ViewModels;

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

        // GET: Muestra la página para asignar ingrediente a una pizza
        public IActionResult AsignarIngredientes(int id)
        {
            var pizza = _context.Pizzas
                .Include(p => p.PizzaIngredientes)
                .FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound();
            }

            var todosLosIngrediente = _context.Ingredientes.ToList();
            var viewModel = new AsignarIngredientesViewModel
            {
                PizzaId = pizza.Id,
                PizzaNombre = pizza.Nombre,
                Ingredientes = todosLosIngrediente.Select(ingrediente => new IngredienteAsignadoViewModel
                {
                    IngredienteId = ingrediente.Id,
                    Nombre = ingrediente.Nombre,
                    // Marcamos el checkbox si la pizza ya tiene este ingrediente asignado
                    Asignado = pizza.PizzaIngredientes.Any(pi => pi.IngredienteId == ingrediente.Id)
                }).ToList()
            };
            return View(viewModel);
        }

        // POST: Guarda los ingredientes seleccionado para una pizza
        [HttpPost]
        public IActionResult AsignarIngredientes(AsignarIngredientesViewModel viewModel)
        {
            var pizza = _context.Pizzas
                .Include(p => p.PizzaIngredientes)
                .FirstOrDefault(p => p.Id == viewModel.PizzaId);

            if (pizza == null)
            {
                return NotFound();
            }

            // Limpiamos las asignaciones anteriores
            pizza.PizzaIngredientes.Clear();

            // Añadimos las nuevas asignaciones basadas en los checkboxes marcados
            foreach (var ingredienteVM in viewModel.Ingredientes)
            {
                if (ingredienteVM.Asignado)
                {
                    pizza.PizzaIngredientes.Add(new PizzaIngrediente
                    {
                        PizzaId = pizza.Id,
                        IngredienteId = ingredienteVM.IngredienteId
                    });                        
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
