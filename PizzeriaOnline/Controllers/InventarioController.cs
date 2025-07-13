using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using PizzeriaOnline.ViewModels;

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ingrediente = _context.Ingredientes.Find(id);
            if (ingrediente != null)
            {
                _context.Ingredientes.Remove(ingrediente);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var ingrediente = _context.Ingredientes.Find(id);
            if (ingrediente == null)
            {
                return NotFound();
            }
            return View(ingrediente);
        }

        public IActionResult Edit(int Id)
        {
            var ingrediente = _context.Ingredientes.Find(Id);

            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ingrediente ingrediente)
        {
            if(ModelState.IsValid)
            {
                _context.Update(ingrediente);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        // POST: Recibe el ViewModel del formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IngredienteCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // "Traducimos" los datos del ViewModel a un nuevo modelo de Ingrediente
                var nuevoIngrediente = new Ingrediente
                {
                    Nombre = viewModel.Nombre,
                    CantidadEnStock = viewModel.CantidadEnStock,
                    UnidadDeMedida = viewModel.UnidadDeMedida
                };

                _context.Add(nuevoIngrediente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            // Si hay un error, volvemos a mostrar la vista con los datos que el usuario ya había llenado
            return View(viewModel);
        }

        // GET: Muestra el formulario usando el ViewModel
        public IActionResult Create()
        {
            return View(new IngredienteCreateViewModel());
        }
    }
}
