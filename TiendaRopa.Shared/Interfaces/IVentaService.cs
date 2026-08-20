using TiendaRopa.Shared.DTOs;

namespace TiendaRopa.Shared.Interfaces;

public interface IVentaService
{
    Task<List<VentaDto>> GetAllAsync();
    Task<VentaDto?> GetByIdAsync(int id);
    Task<VentaDto?> GetByFacturaAsync(string factura);
    Task<VentaDto> CreateAsync(VentaCreateDto dto);
    Task<DashboardDto> GetDashboardAsync();
}
