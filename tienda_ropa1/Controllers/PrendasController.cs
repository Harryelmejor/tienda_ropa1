using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;

namespace tienda_ropa1.Controllers;

public class PrendasController : Controller
{
    private readonly TiendaDbContext _context;

    public PrendasController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar, int? categoriaId)
    {
        var query = _context.Prendas.Include(p => p.Categoria).AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(p => p.Nombre.Contains(buscar) || (p.Talla != null && p.Talla.Contains(buscar)));

        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId);

        ViewBag.Categorias = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", categoriaId);
        ViewBag.Buscar = buscar;
        ViewBag.CategoriaId = categoriaId;

        return View(await query.OrderBy(p => p.Nombre).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var prenda = await _context.Prendas
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (prenda == null) return NotFound();
        return View(prenda);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.CategoriaId = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Prenda prenda)
    {
        if (ModelState.IsValid)
        {
            _context.Add(prenda);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Prenda registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoriaId = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", prenda.CategoriaId);
        return View(prenda);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null) return NotFound();
        ViewBag.CategoriaId = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", prenda.CategoriaId);
        return View(prenda);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Prenda prenda)
    {
        if (id != prenda.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(prenda);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Prenda actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoriaId = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", prenda.CategoriaId);
        return View(prenda);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var prenda = await _context.Prendas
            .Include(p => p.Categoria)
            .Include(p => p.DetalleVentas)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (prenda == null) return NotFound();
        return View(prenda);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null) return NotFound();

        var tieneVentas = await _context.DetalleVentas.AnyAsync(d => d.PrendaId == id);
        if (tieneVentas)
        {
            TempData["Error"] = "No se puede eliminar una prenda con ventas registradas.";
            return RedirectToAction(nameof(Index));
        }

        _context.Prendas.Remove(prenda);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Prenda eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
