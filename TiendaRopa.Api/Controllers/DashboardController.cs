using Microsoft.AspNetCore.Mvc;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IVentaService _ventaService;
    private readonly IPrendaService _prendaService;
    private readonly IClienteService _clienteService;
    private readonly IEmpleadoService _empleadoService;

    public DashboardController(IVentaService ventaService, IPrendaService prendaService, IClienteService clienteService, IEmpleadoService empleadoService)
    {
        _ventaService = ventaService;
        _prendaService = prendaService;
        _clienteService = clienteService;
        _empleadoService = empleadoService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        var prendas = await _prendaService.GetAllAsync();
        var clientes = await _clienteService.GetAllAsync();
        var empleados = await _empleadoService.GetAllAsync();
        var ventas = await _ventaService.GetAllAsync();
        var stockBajo = await _prendaService.GetStockBajoAsync();
        var ventasMes = ventas.Where(v => v.Fecha.Month == DateTime.Now.Month && v.Fecha.Year == DateTime.Now.Year).Sum(v => v.Total);

        return Ok(new DashboardDto
        {
            TotalPrendas = prendas.Count,
            TotalClientes = clientes.Count,
            TotalEmpleados = empleados.Count,
            TotalVentas = ventas.Count,
            InventarioTotal = prendas.Sum(p => p.Stock),
            VentasMes = ventasMes,
            VentasRecientes = ventas.Take(5).ToList(),
            StockBajo = stockBajo
        });
    }
}
