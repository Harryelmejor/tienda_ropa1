using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Data;
using tienda_ropa1.Models;

namespace tienda_ropa1.Controllers;

public class EmpleadosController : Controller
{
    private readonly TiendaDbContext _context;

    public EmpleadosController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var query = _context.Empleados.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(e =>
                e.Nombre.Contains(buscar) ||
                e.Apellido.Contains(buscar) ||
                (e.Cargo != null && e.Cargo.Contains(buscar)));

        ViewBag.Buscar = buscar;
        return View(await query.OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var empleado = await _context.Empleados
            .Include(e => e.Ventas)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (empleado == null) return NotFound();
        return View(empleado);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Empleado empleado)
    {
        if (ModelState.IsValid)
        {
            _context.Add(empleado);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Empleado registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(empleado);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado == null) return NotFound();
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Empleado empleado)
    {
        if (id != empleado.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(empleado);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Empleado actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(empleado);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var empleado = await _context.Empleados
            .Include(e => e.Ventas)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (empleado == null) return NotFound();
        return View(empleado);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var empleado = await _context.Empleados
            .Include(e => e.Ventas)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (empleado == null) return NotFound();

        if (empleado.Ventas.Any())
        {
            TempData["Error"] = "No se puede eliminar un empleado con ventas registradas.";
            return RedirectToAction(nameof(Index));
        }

        _context.Empleados.Remove(empleado);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Empleado eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
