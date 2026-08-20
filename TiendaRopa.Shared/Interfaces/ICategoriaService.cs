using TiendaRopa.Shared.DTOs;

namespace TiendaRopa.Shared.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> GetByIdAsync(int id);
    Task<CategoriaDto> CreateAsync(CategoriaCreateDto dto);
    Task<CategoriaDto?> UpdateAsync(int id, CategoriaCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
