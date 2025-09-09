using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class TamañosController : Controller
{
    private readonly ApplicationDbContext _context;

    public TamañosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Tamaños
    public async Task<IActionResult> Index()
    {
        var tamaños = await _context.Tamaños.ToListAsync();
        return View(tamaños);
    }

    // GET: /Tamaños/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tamaños/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tamaño tamaño)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tamaño);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tamaño);
    }

    // GET: /Tamaños/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tamaño = await _context.Tamaños.FindAsync(id);
        if (tamaño == null) return NotFound();
        return View(tamaño);
    }

    // POST: /Tamaños/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tamaño tamaño)
    {
        if (id != tamaño.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tamaño);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tamaños.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tamaño);
    }

    // GET: /Tamaños/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tamaño = await _context.Tamaños.FirstOrDefaultAsync(m => m.Id == id);
        if (tamaño == null) return NotFound();
        return View(tamaño);
    }

    // POST: /Tamaños/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tamaño = await _context.Tamaños.FindAsync(id);
        if (tamaño != null)
        {
            _context.Tamaños.Remove(tamaño);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}