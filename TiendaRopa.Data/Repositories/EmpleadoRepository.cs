using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data.Repositories;

public class EmpleadoRepository : IEmpleadoService
{
    private readonly TiendaDbContext _context;
    public EmpleadoRepository(TiendaDbContext context) => _context = context;

    public async Task<List<EmpleadoDto>> GetAllAsync(string? buscar = null)
    {
        var query = _context.Empleados.AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(e => e.Nombre.Contains(buscar) || e.Apellido.Contains(buscar) || (e.Cargo != null && e.Cargo.Contains(buscar)));
        return await query.Select(e => new EmpleadoDto
        {
            Id = e.Id, Nombre = e.Nombre, Apellido = e.Apellido,
            Telefono = e.Telefono, Email = e.Email, Cargo = e.Cargo, CantidadVentas = e.Ventas.Count
        }).OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToListAsync();
    }

    public async Task<EmpleadoDto?> GetByIdAsync(int id)
    {
        return await _context.Empleados.Where(e => e.Id == id)
            .Select(e => new EmpleadoDto
            {
                Id = e.Id, Nombre = e.Nombre, Apellido = e.Apellido,
                Telefono = e.Telefono, Email = e.Email, Cargo = e.Cargo, CantidadVentas = e.Ventas.Count
            }).FirstOrDefaultAsync();
    }

    public async Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto)
    {
        var emp = new Empleado { Nombre = dto.Nombre, Apellido = dto.Apellido, Telefono = dto.Telefono, Email = dto.Email, Cargo = dto.Cargo };
        _context.Empleados.Add(emp);
        await _context.SaveChangesAsync();
        return new EmpleadoDto { Id = emp.Id, Nombre = emp.Nombre, Apellido = emp.Apellido, Telefono = emp.Telefono, Email = emp.Email, Cargo = emp.Cargo };
    }

    public async Task<EmpleadoDto?> UpdateAsync(int id, EmpleadoCreateDto dto)
    {
        var emp = await _context.Empleados.FindAsync(id);
        if (emp == null) return null;
        emp.Nombre = dto.Nombre; emp.Apellido = dto.Apellido;
        emp.Telefono = dto.Telefono; emp.Email = dto.Email; emp.Cargo = dto.Cargo;
        await _context.SaveChangesAsync();
        return new EmpleadoDto { Id = emp.Id, Nombre = emp.Nombre, Apellido = emp.Apellido, Telefono = emp.Telefono, Email = emp.Email, Cargo = emp.Cargo };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var emp = await _context.Empleados.FindAsync(id);
        if (emp == null) return false;
        if (await _context.Ventas.AnyAsync(v => v.EmpleadoId == id)) return false;
        _context.Empleados.Remove(emp);
        await _context.SaveChangesAsync();
        return true;
    }
}
