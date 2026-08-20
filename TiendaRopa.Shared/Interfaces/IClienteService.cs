using TiendaRopa.Shared.DTOs;

namespace TiendaRopa.Shared.Interfaces;

public interface IClienteService
{
    Task<List<ClienteDto>> GetAllAsync(string? buscar = null);
    Task<ClienteDto?> GetByIdAsync(int id);
    Task<ClienteDto> CreateAsync(ClienteCreateDto dto);
    Task<ClienteDto?> UpdateAsync(int id, ClienteCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
