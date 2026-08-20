using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data.Repositories;

public class VentaRepository : IVentaService
{
    private readonly TiendaDbContext _context;
    public VentaRepository(TiendaDbContext context) => _context = context;

    public async Task<List<VentaDto>> GetAllAsync()
    {
        return await _context.Ventas
            .Include(v => v.Cliente).Include(v => v.Empleado)
            .Include(v => v.DetalleVentas).ThenInclude(d => d.Prenda)
            .OrderByDescending(v => v.Fecha)
            .Select(v => new VentaDto
            {
                Id = v.Id, ClienteId = v.ClienteId,
                ClienteNombre = v.Cliente != null ? v.Cliente.Nombre + " " + v.Cliente.Apellido : null,
                EmpleadoId = v.EmpleadoId, EmpleadoNombre = v.Empleado.Nombre + " " + v.Empleado.Apellido,
                Fecha = v.Fecha, Total = v.Total,
                Detalles = v.DetalleVentas.Select(d => new DetalleVentaDto
                {
                    Id = d.Id, PrendaId = d.PrendaId, PrendaNombre = d.Prenda!.Nombre,
                    PrendaTalla = d.Prenda.Talla, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario
                }).ToList()
            }).ToListAsync();
    }

    public async Task<VentaDto?> GetByIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Cliente).Include(v => v.Empleado)
            .Include(v => v.DetalleVentas).ThenInclude(d => d.Prenda)
            .Where(v => v.Id == id)
            .Select(v => new VentaDto
            {
                Id = v.Id, ClienteId = v.ClienteId,
                ClienteNombre = v.Cliente != null ? v.Cliente.Nombre + " " + v.Cliente.Apellido : null,
                EmpleadoId = v.EmpleadoId, EmpleadoNombre = v.Empleado.Nombre + " " + v.Empleado.Apellido,
                Fecha = v.Fecha, Total = v.Total,
                Detalles = v.DetalleVentas.Select(d => new DetalleVentaDto
                {
                    Id = d.Id, PrendaId = d.PrendaId, PrendaNombre = d.Prenda!.Nombre,
                    PrendaTalla = d.Prenda.Talla, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<VentaDto?> GetByFacturaAsync(string factura)
    {
        if (!factura.StartsWith("VTA-") || !int.TryParse(factura[4..], out var num)) return null;
        return await GetByIdAsync(num);
    }

    public async Task<VentaDto> CreateAsync(VentaCreateDto dto)
    {
        if (!dto.Items.Any()) throw new InvalidOperationException("Debe agregar al menos una prenda.");
        var prendaIds = dto.Items.Select(i => i.PrendaId).ToList();
        var prendas = await _context.Prendas.Where(p => prendaIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id);

        decimal total = 0;
        var venta = new Venta { ClienteId = dto.ClienteId > 0 ? dto.ClienteId : null, EmpleadoId = dto.EmpleadoId, Fecha = DateTime.Now, Total = 0 };

        foreach (var item in dto.Items)
        {
            if (!prendas.TryGetValue(item.PrendaId, out var prenda))
                throw new InvalidOperationException($"Prenda {item.PrendaId} no encontrada.");
            if (prenda.Stock < item.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente para '{prenda.Nombre}' (disponible: {prenda.Stock}).");

            var detalle = new DetalleVenta { PrendaId = item.PrendaId, Cantidad = item.Cantidad, PrecioUnitario = prenda.Precio };
            venta.DetalleVentas.Add(detalle);
            total += detalle.Cantidad * detalle.PrecioUnitario;
            prenda.Stock -= item.Cantidad;
        }

        venta.Total = total;
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(venta.Id))!;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var ventasMes = await _context.Ventas
            .Where(v => v.Fecha.Month == DateTime.Now.Month && v.Fecha.Year == DateTime.Now.Year)
            .SumAsync(v => v.Total);
        return new DashboardDto
        {
            TotalPrendas = await _context.Prendas.CountAsync(),
            TotalClientes = await _context.Clientes.CountAsync(),
            TotalEmpleados = await _context.Empleados.CountAsync(),
            TotalVentas = await _context.Ventas.CountAsync(),
            InventarioTotal = await _context.Prendas.SumAsync(p => p.Stock),
            VentasMes = ventasMes,
            VentasRecientes = await GetAllAsync(),
            StockBajo = await ((IPrendaService)new PrendaRepository(_context)).GetStockBajoAsync()
        };
    }
}
