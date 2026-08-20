using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Data;
using tienda_ropa1.Models;
using tienda_ropa1.Models.ViewModels;

namespace tienda_ropa1.Controllers;

public class VentasController : Controller
{
    private readonly TiendaDbContext _context;

    public VentasController(TiendaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Empleado)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await LoadCreateViewBags();
        return View(new VentaCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VentaCreateViewModel model)
    {
        model.Items = model.Items.Where(i => i.PrendaId > 0).ToList();

        if (!model.Items.Any())
        {
            ModelState.AddModelError("", "Debe agregar al menos una prenda a la venta.");
        }

        if (!ModelState.IsValid)
        {
            await LoadCreateViewBags();
            return View(model);
        }

        var prendaIds = model.Items.Select(i => i.PrendaId).ToList();
        var prendas = await _context.Prendas
            .Where(p => prendaIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var item in model.Items)
        {
            if (!prendas.TryGetValue(item.PrendaId, out var prenda))
            {
                ModelState.AddModelError("", "Una de las prendas seleccionadas no existe.");
                await LoadCreateViewBags();
                return View(model);
            }

            if (prenda.Stock < item.Cantidad)
            {
                ModelState.AddModelError("", $"Stock insuficiente para '{prenda.Nombre}' (disponible: {prenda.Stock}).");
                await LoadCreateViewBags();
                return View(model);
            }
        }

        var venta = new Venta
        {
            ClienteId = model.ClienteId > 0 ? model.ClienteId : null,
            EmpleadoId = model.EmpleadoId,
            Fecha = DateTime.Now,
            Total = 0
        };

        decimal total = 0;

        foreach (var item in model.Items)
        {
            var prenda = prendas[item.PrendaId];
            var detalle = new DetalleVenta
            {
                PrendaId = item.PrendaId,
                Cantidad = item.Cantidad,
                PrecioUnitario = prenda.Precio
            };
            venta.DetalleVentas.Add(detalle);
            total += detalle.Cantidad * detalle.PrecioUnitario;
            prenda.Stock -= item.Cantidad;
        }

        venta.Total = total;
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Venta registrada. Factura {venta.NumeroFactura} generada.";
        return RedirectToAction(nameof(Factura), new { id = venta.Id });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var venta = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Empleado)
            .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Prenda)
                    .ThenInclude(p => p!.Categoria)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null) return NotFound();
        return View(venta);
    }

    public async Task<IActionResult> Factura(int? id)
    {
        if (id == null) return NotFound();

        var venta = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Empleado)
            .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Prenda)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null) return NotFound();
        return View(venta);
    }

    private async Task LoadCreateViewBags()
    {
        ViewBag.ClienteId = new SelectList(
            await _context.Clientes.OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync(),
            "Id", "NombreCompleto");

        ViewBag.EmpleadoId = new SelectList(
            await _context.Empleados.OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToListAsync(),
            "Id", "NombreCompleto");

        ViewBag.Prendas = await _context.Prendas
            .Include(p => p.Categoria)
            .Where(p => p.Stock > 0)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }
}
