using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Data;
using tienda_ropa1.Models;

namespace tienda_ropa1.Controllers;

public class ClientesController : Controller
{
    private readonly TiendaDbContext _context;

    public ClientesController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var query = _context.Clientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(c =>
                c.Nombre.Contains(buscar) ||
                c.Apellido.Contains(buscar) ||
                (c.Email != null && c.Email.Contains(buscar)));

        ViewBag.Buscar = buscar;
        return View(await query.OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes
            .Include(c => c.Ventas)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cliente registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(cliente);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes
            .Include(c => c.Ventas)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Ventas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null) return NotFound();

        if (cliente.Ventas.Any())
        {
            TempData["Error"] = "No se puede eliminar un cliente con ventas registradas.";
            return RedirectToAction(nameof(Index));
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Cliente eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
