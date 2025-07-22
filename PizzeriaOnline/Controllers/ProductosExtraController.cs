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
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductosExtraController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create(ProductoExtraCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string nombreUnicoArchivo = null;

                // 1. Proceso de guardado del archivo
                if (viewModel.ImagenArchivo != null)
                {
                    // Obtenemos la ruta a la carpeta wwwroot
                    string carpetaUploads = Path.Combine(_hostEnvironment.WebRootPath, "images/productos");

                    if (!Directory.Exists(carpetaUploads))
                    {
                        Directory.CreateDirectory(carpetaUploads);
                    }
                    // Creamos un nombre de archivo único para evitar colisiones
                    nombreUnicoArchivo = Guid.NewGuid().ToString() + "_" + viewModel.ImagenArchivo.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreUnicoArchivo);

                    // Usamos un 'using' para asegurarnos de que el archivo se cierre correctamente
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await viewModel.ImagenArchivo.CopyToAsync(fileStream);
                    }
                }

                // 2. "Traducimos" del viewModel al Modelo de la BD
                var nuevoProducto = new ProductoExtra
                {
                    Nombre = viewModel.Nombre,
                    Precio = viewModel.Precio,
                    CantidadEnStock = viewModel.CantidadEnStock,
                    // Guardamos solo la ruta relativa en la base de datos
                    RutaImagen = "/images/productos/" + nombreUnicoArchivo
                };               

                _context.Add(nuevoProducto);
                await _context.SaveChangesAsync();
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
