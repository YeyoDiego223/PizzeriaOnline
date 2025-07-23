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
            var producto = _context.ProductoExtras.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            // "Traducimos del modelo de la BD a la ViewModel de edicion"
            var viewModel = new ProductoExtraEditViewModel
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CantidadEnStock = producto.CantidadEnStock,
                RutaImagenExistente = producto.RutaImagen
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoExtraEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Buscamos el productos original en la base de datos
                var producto = _context.ProductoExtras.Find(viewModel.Id);
                if (producto == null)
                {
                    return NotFound();
                }

                string nombreUnicoArchivo = viewModel.RutaImagenExistente;

                // Si el usuario subio una NUEVA imagen
                if (viewModel.ImagenArchivo != null)
                {
                    // (Opcional pero recomendado) Borrar la imagen antigua si existe
                    if (!string.IsNullOrEmpty(viewModel.RutaImagenExistente))
                    {
                        string rutaAntigua = Path.Combine(_hostEnvironment.WebRootPath, viewModel.RutaImagenExistente.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAntigua))
                        {
                            System.IO.File.Delete(rutaAntigua);
                        }
                    }

                    // Guardar la nueva imagen
                    string carpetaUploads = Path.Combine(_hostEnvironment.WebRootPath, "images/productos");
                    nombreUnicoArchivo = Guid.NewGuid().ToString() + "_" + viewModel.ImagenArchivo.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreUnicoArchivo);
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await viewModel.ImagenArchivo.CopyToAsync(fileStream);
                    }
                    nombreUnicoArchivo = "/images/productos/" + nombreUnicoArchivo; // Guardamos la ruta relativa
                }

                // Actualizamos los datos del producto con la información del ViewModel
                producto.Nombre = viewModel.Nombre;
                producto.Precio = viewModel.Precio;
                producto.CantidadEnStock = viewModel.CantidadEnStock;
                producto.RutaImagen = nombreUnicoArchivo;

                _context.Update(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
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
