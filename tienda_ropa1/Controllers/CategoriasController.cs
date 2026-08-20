using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Data;
using tienda_ropa1.Models;

namespace tienda_ropa1.Controllers;

public class CategoriasController : Controller
{
    private readonly TiendaDbContext _context;

    public CategoriasController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Categorias
            .Include(c => c.Prendas)
            .OrderBy(c => c.Nombre)
            .ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Categoría registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Categoría actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var categoria = await _context.Categorias
            .Include(c => c.Prendas)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (categoria == null) return NotFound();
        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Prendas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null) return NotFound();

        if (categoria.Prendas.Any())
        {
            TempData["Error"] = "No se puede eliminar una categoría con prendas asociadas.";
            return RedirectToAction(nameof(Index));
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Categoría eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
