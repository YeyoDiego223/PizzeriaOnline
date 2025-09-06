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
            _context.Update(viewModel);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "¡Configuración guardad con éxito!";
            return RedirectToAction("Index", "Admin");
        }
        return View(viewModel);
    }
}

