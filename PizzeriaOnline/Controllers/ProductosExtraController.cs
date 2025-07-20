using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using PizzeriaOnline.ViewModels;


namespace PizzeriaOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosExtraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosExtraController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listadeProductos = _context.ProductoExtras.ToList();
            return View(listadeProductos);
        }


        public IActionResult Create()
        {
            return View(new ProductoExtraCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductoExtraCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var nuevoProductoExtra = new ProductoExtra
                {
                    Nombre = viewModel.Nombre,
                    Precio = viewModel.Precio,
                    CantidadEnStock = viewModel.CantidadEnStock,
                };

                _context.Add(nuevoProductoExtra);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            var productoExtra = _context.ProductoExtras.Find(id);

            if (productoExtra == null)
            {
                return NotFound();
            }
            return View(productoExtra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductoExtra productoExtra)
        {
            if (ModelState.IsValid)
            {
                _context.Update(productoExtra);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
       

        public IActionResult Delete(int id)
        {
            var productoExtra = _context.ProductoExtras.Find(id);

            if (productoExtra == null)
            {
                return NotFound();
            }
            return View(productoExtra);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var productoExtra = _context.ProductoExtras.Find(id);
            if (productoExtra != null)
            {
                _context.ProductoExtras.Remove(productoExtra);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
