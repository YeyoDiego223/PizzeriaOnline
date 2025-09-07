using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Threading.Tasks;


[Authorize(Roles = "Admin")]
public class ConfiguracionController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConfiguracionController(ApplicationDbContext context)
    {
        _context = context;
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
    public async Task<IActionResult> Index(Configuracion viewModel)
    {
        if (ModelState.IsValid)
        {
            var configEnDB = await _context.Configuracion.FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            if (configEnDB != null)
            {
                configEnDB.ForzarCierre = viewModel.ForzarCierre;
                configEnDB.HoraApertura = viewModel.HoraApertura;
                configEnDB.HoraCierre = viewModel.HoraCierre;

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

