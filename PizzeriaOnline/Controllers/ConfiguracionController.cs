using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Threading.Tasks;


[Authorize(Roles = "Admin")]
public class ConfiguracionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ConfiguracionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var config = await _context.Configuracion.FirstOrDefaultAsync();
        if (config == null)
        {
            return NotFound("No se encontró la configuración inicial.");
        }
        return View(config);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Configuracion viewModel, IFormFile? imagenPromocion)
    {
        if (ModelState.IsValid)
        {
            var configEnDB = await _context.Configuracion.FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            if (configEnDB != null)
            {
                configEnDB.ForzarCierre = viewModel.ForzarCierre;
                configEnDB.HoraApertura = viewModel.HoraApertura;
                configEnDB.HoraCierre = viewModel.HoraCierre;

                configEnDB.PromocionEstaActiva = viewModel.PromocionEstaActiva;
                configEnDB.PromocionTitulo = viewModel.PromocionTitulo;
                configEnDB.PromocionDescripcion = viewModel.PromocionDescripcion;
                configEnDB.PromocionPrecio = viewModel.PromocionPrecio;

                // Procesa la imagen si se subio una nueva
                if (imagenPromocion != null)
                {
                    // (Opcional) Borrar imagen antigua
                    if (!string.IsNullOrEmpty(configEnDB.PromocionRutaImagen))
                    {
                        string rutaAntigua = Path.Combine(_hostEnvironment.WebRootPath, configEnDB.PromocionRutaImagen.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAntigua))
                        {
                            System.IO.File.Delete(rutaAntigua);
                        }
                    }

                    // Guardar nueva imagen
                    string carpetaUploads = Path.Combine(_hostEnvironment.WebRootPath, "images/promociones");
                    string nombreUnico = Guid.NewGuid().ToString() + "_" + imagenPromocion.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreUnico);
                    using (var fs = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await imagenPromocion.CopyToAsync(fs);
                    }
                    configEnDB.PromocionRutaImagen = "/images/promociones/" + nombreUnico;

                }


                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "¡Configuración guardada con éxito!";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "No se encontró el registro de configuracion para actualizar.");
            }            
        }
        return View(viewModel);
    }
}

