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

        // GET: /PizzasAdmin/ElegirTamañoParaReceta/5
        public IActionResult ElegirTamañoParaReceta(int Id)
        {
            var pizza = _context.Pizzas.Find(Id);
            if (pizza == null)
            {
                return NotFound();
            }

            // Usamos ViewBag para pasar la información extra (la pizza) a la vista.
            ViewBag.Pizza = pizza;

            // Pasamos la lista de todos los tamaños como el modelo principal de la vista.
            var tamaños = _context.Tamaños.ToList().OrderBy(t => t.PrecioBase).ToList();

            return View(tamaños);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GestionarReceta(GestionarRecetaViewModel viewModel)
        {
            // 1. Buscamos y eliminamos las entradas de la receta anterior para esta pizza y tamaño.
            var recetaAnterior = _context.Recetas
                .Where(r => r.PizzaId == viewModel.PizzaId && r.TamañoId == viewModel.TamañoId);

            if (recetaAnterior.Any())
            {
                _context.Recetas.RemoveRange(recetaAnterior);
            }

            // 2. Recorremos los ingredientes que nos llegaron del formulario.
            if (viewModel.Ingredientes != null)
            {
                foreach (var ingredienteVM in viewModel.Ingredientes)
                {
                    // 3. Si el ingrediente esta marcado (Adignado == true) y tiene una cantidad mayor a caro...
                    if (ingredienteVM.Asignado && ingredienteVM.Cantidad > 0)
                    {
                        // ...creamos un nuevo registro de Receta con su cantidad.
                        var nuevaReceta = new Receta
                        {
                            PizzaId = viewModel.PizzaId,
                            TamañoId = viewModel.TamañoId,
                            IngredienteId = ingredienteVM.IngredienteId,
                            Cantidad = ingredienteVM.Cantidad,
                        };
                        _context.Recetas.Add(nuevaReceta);
                    }
                }
            }            

            // 4. Guardamos todos los cambios (el borrado y las nuevas inserciones) en una sola transacción.
            _context.SaveChanges();

            // 5. Redirigimos al usuario de vuelta a la lista de pizzas.
            return RedirectToAction(nameof(Index));
        }

        // GET: Muestra la página para gestionar la receta de una pizza en un tamaño especifico
        public IActionResult GestionarReceta(int pizzaId, int tamañoId)
        {
            // Buscamos la pizza y el tamaño para tener sus nombres
            var pizza = _context.Pizzas.Find(pizzaId);
            var tamaño = _context.Tamaños.Find(tamañoId);

            if (pizza == null || tamaño == null)
            {
                return NotFound();
            }

            // GET: /PizzasAdmin/Elegir

            // Obtenemos la lista de TODOS los ingredientes disponibles
            var todosLosIngredientes = _context.Ingredientes.ToList();

            var viewModel = new GestionarRecetaViewModel
            {
                PizzaId = pizza.Id,
                PizzaNombre = pizza.Nombre,
                TamañoId = tamaño.Id,
                TamañoNombre = tamaño.Nombre,
                // Creamos la lista de ingredientes para el formulario
                Ingredientes = todosLosIngredientes.Select(ing =>
                {
                    // Buscamos si ya existe una receta para este ingrediente, pizza y tamaño
                    var recetaExistente = _context.Recetas
                        .FirstOrDefault(r => r.PizzaId == pizzaId && r.TamañoId == tamañoId && r.IngredienteId == ing.Id);

                    // Creamos la línea del formulario
                    return new IngredienteRecetaViewModel
                    {
                        IngredienteId = ing.Id,
                        Nombre = ing.Nombre,
                        UnidadDeMedida = ing.UnidadDeMedida,
                        // Si la receta existe, marcamos el checkbox
                        Asignado = recetaExistente != null,
                        // Si la receta existe, ponemos la cantidad guardada, si no, ponemos 0
                        Cantidad = recetaExistente?.Cantidad ?? 0
                    };
                }).ToList()
            };

            return View(viewModel);
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
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View(new PizzaCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // "Traducimos" del ViewModel al Modelo de la base de datos
                var nuevaPizza = new Pizza
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    RutaImagen = viewModel.RutaImagen
                };

                _context.Add(nuevaPizza);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public IActionResult Index()
        {
            var listadePizzas = _context.Pizzas.ToList();
            return View(listadePizzas);
        }
    }
}
