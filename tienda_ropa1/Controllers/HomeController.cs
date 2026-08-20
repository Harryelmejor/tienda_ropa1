using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Data;

namespace tienda_ropa1.Controllers;

public class HomeController : Controller
{
    private readonly TiendaDbContext _context;

    public HomeController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalPrendas = await _context.Prendas.CountAsync();
        ViewBag.TotalClientes = await _context.Clientes.CountAsync();
        ViewBag.TotalVentas = await _context.Ventas.CountAsync();
        ViewBag.InventarioTotal = await _context.Prendas.SumAsync(p => p.Stock);
        ViewBag.VentasRecientes = await _context.Ventas
            .Include(v => v.Cliente)
            .OrderByDescending(v => v.Fecha)
            .Take(5)
            .ToListAsync();
        ViewBag.StockBajo = await _context.Prendas
            .Include(p => p.Categoria)
            .Where(p => p.Stock <= 10)
            .OrderBy(p => p.Stock)
            .Take(5)
            .ToListAsync();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
