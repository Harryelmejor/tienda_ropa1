using TiendaRopa.Shared.DTOs;

namespace TiendaRopa.Shared.Interfaces;

public interface IEmpleadoService
{
    Task<List<EmpleadoDto>> GetAllAsync(string? buscar = null);
    Task<EmpleadoDto?> GetByIdAsync(int id);
    Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto);
    Task<EmpleadoDto?> UpdateAsync(int id, EmpleadoCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
