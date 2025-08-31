using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using PizzeriaOnline.ViewModels;

namespace PizzeriaOnline.Controllers
{

    [Authorize(Roles = "Admin")]
    public class PromocionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PromocionesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var listadodePromociones = _context.Promociones.ToList();
            return View(listadodePromociones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromocionCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string nombreUnicoArchivo = null;

                if (viewModel.RutaImagen != null)
                {
                    string carpetaUploads = Path.Combine(_hostEnvironment.WebRootPath, "images/promociones");

                    if (!Directory.Exists(carpetaUploads))
                    {
                        Directory.CreateDirectory(carpetaUploads);
                    }
                    nombreUnicoArchivo = Guid.NewGuid().ToString() + "_" + viewModel.RutaImagen.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreUnicoArchivo);

                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await viewModel.RutaImagen.CopyToAsync(fileStream);
                    }
                }

                var nuevaPromocion = new Promocion
                {
                    Titulo = viewModel.Titulo,
                    Descripcion = viewModel.Descripcion,
                    Precio = viewModel.Precio,
                    RutaImagen = "/images/promociones/" + nombreUnicoArchivo,
                    EstaActiva = viewModel.EstaActiva
                };

                _context.Add(nuevaPromocion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new PromocionCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PromocionEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var promocion = _context.Promociones.Find(viewModel.Id);
                if (promocion == null)
                {
                    return NotFound();
                }

                string nombreUnicoArchivo = viewModel.RutaImagenExistente;

                if (viewModel.ImagenArchivo != null)
                {
                    if (!string.IsNullOrEmpty(viewModel.RutaImagenExistente))
                    {
                        string rutaAntigua = Path.Combine(_hostEnvironment.WebRootPath, viewModel.RutaImagenExistente.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAntigua))
                        {
                            System.IO.File.Delete(rutaAntigua);
                        }
                    }

                    string carpetaUploads = Path.Combine(_hostEnvironment.WebRootPath, "images/promociones");
                    nombreUnicoArchivo = Guid.NewGuid().ToString() + "_" + viewModel.ImagenArchivo.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreUnicoArchivo);
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await viewModel.ImagenArchivo.CopyToAsync(fileStream);
                    }
                    nombreUnicoArchivo = "/images/promociones/" + nombreUnicoArchivo;
                }

                promocion.Titulo = viewModel.Titulo;
                promocion.Descripcion = viewModel.Descripcion;
                promocion.Precio = viewModel.Precio;
                promocion.RutaImagen = nombreUnicoArchivo;
                promocion.EstaActiva = viewModel.EstaActiva;
                
                _context.Update(promocion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);            
        }

        public IActionResult Edit(int id)
        {
            var promocion = _context.Promociones.Find(id);
            if (promocion == null)
            {
                return NotFound();
            }

            var viewModel = new PromocionEditViewModel
            {
                Id = promocion.Id,
                Titulo = promocion.Titulo,
                Descripcion = promocion.Descripcion,
                Precio = promocion.Precio,
                RutaImagenExistente = promocion.RutaImagen,
                EstaActiva = promocion.EstaActiva
            };

            return View(viewModel);
        }
        
        public IActionResult Delete(int id)
        {
            var promocion = _context.Promociones.Find(id);
            if (promocion == null)
            {
                return NotFound();
            }
            return View(promocion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var promocion = _context.Promociones.Find(id);
            if (promocion != null)
            {
                _context.Promociones.Remove(promocion);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
