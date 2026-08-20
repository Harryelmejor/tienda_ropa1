using TiendaRopa.Shared.DTOs;

namespace TiendaRopa.Shared.Interfaces;

public interface IPrendaService
{
    Task<List<PrendaDto>> GetAllAsync(string? buscar = null, int? categoriaId = null);
    Task<PrendaDto?> GetByIdAsync(int id);
    Task<PrendaDto> CreateAsync(PrendaCreateDto dto);
    Task<PrendaDto?> UpdateAsync(int id, PrendaCreateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<PrendaDto>> GetStockBajoAsync(int umbral = 10);
}
