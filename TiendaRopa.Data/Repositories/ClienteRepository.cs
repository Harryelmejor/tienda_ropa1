using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data.Repositories;

public class ClienteRepository : IClienteService
{
    private readonly TiendaDbContext _context;
    public ClienteRepository(TiendaDbContext context) => _context = context;

    public async Task<List<ClienteDto>> GetAllAsync(string? buscar = null)
    {
        var query = _context.Clientes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(c => c.Nombre.Contains(buscar) || c.Apellido.Contains(buscar) || (c.Email != null && c.Email.Contains(buscar)));
        return await query.Select(c => new ClienteDto
        {
            Id = c.Id, Nombre = c.Nombre, Apellido = c.Apellido,
            Telefono = c.Telefono, Email = c.Email, Direccion = c.Direccion, CantidadVentas = c.Ventas.Count
        }).OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync();
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        return await _context.Clientes.Where(c => c.Id == id)
            .Select(c => new ClienteDto
            {
                Id = c.Id, Nombre = c.Nombre, Apellido = c.Apellido,
                Telefono = c.Telefono, Email = c.Email, Direccion = c.Direccion, CantidadVentas = c.Ventas.Count
            }).FirstOrDefaultAsync();
    }

    public async Task<ClienteDto> CreateAsync(ClienteCreateDto dto)
    {
        var cliente = new Cliente { Nombre = dto.Nombre, Apellido = dto.Apellido, Telefono = dto.Telefono, Email = dto.Email, Direccion = dto.Direccion };
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return new ClienteDto { Id = cliente.Id, Nombre = cliente.Nombre, Apellido = cliente.Apellido, Telefono = cliente.Telefono, Email = cliente.Email, Direccion = cliente.Direccion };
    }

    public async Task<ClienteDto?> UpdateAsync(int id, ClienteCreateDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return null;
        cliente.Nombre = dto.Nombre; cliente.Apellido = dto.Apellido;
        cliente.Telefono = dto.Telefono; cliente.Email = dto.Email; cliente.Direccion = dto.Direccion;
        await _context.SaveChangesAsync();
        return new ClienteDto { Id = cliente.Id, Nombre = cliente.Nombre, Apellido = cliente.Apellido, Telefono = cliente.Telefono, Email = cliente.Email, Direccion = cliente.Direccion };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;
        if (await _context.Ventas.AnyAsync(v => v.ClienteId == id)) return false;
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
}
