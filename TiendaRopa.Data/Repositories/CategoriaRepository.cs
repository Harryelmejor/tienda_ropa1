using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data.Repositories;

public class CategoriaRepository : ICategoriaService
{
    private readonly TiendaDbContext _context;

    public CategoriaRepository(TiendaDbContext context) => _context = context;

    public async Task<List<CategoriaDto>> GetAllAsync()
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto
            {
                Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion,
                CantidadPrendas = c.Prendas.Count
            })
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<CategoriaDto?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .Select(c => new CategoriaDto
            {
                Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion,
                CantidadPrendas = c.Prendas.Count
            })
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CategoriaDto> CreateAsync(CategoriaCreateDto dto)
    {
        var cat = new Categoria { Nombre = dto.Nombre, Descripcion = dto.Descripcion };
        _context.Categorias.Add(cat);
        await _context.SaveChangesAsync();
        return new CategoriaDto { Id = cat.Id, Nombre = cat.Nombre, Descripcion = cat.Descripcion };
    }

    public async Task<CategoriaDto?> UpdateAsync(int id, CategoriaCreateDto dto)
    {
        var cat = await _context.Categorias.FindAsync(id);
        if (cat == null) return null;
        cat.Nombre = dto.Nombre;
        cat.Descripcion = dto.Descripcion;
        await _context.SaveChangesAsync();
        return new CategoriaDto { Id = cat.Id, Nombre = cat.Nombre, Descripcion = cat.Descripcion };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cat = await _context.Categorias.FindAsync(id);
        if (cat == null) return false;
        if (await _context.Prendas.AnyAsync(p => p.CategoriaId == id))
            return false;
        _context.Categorias.Remove(cat);
        await _context.SaveChangesAsync();
        return true;
    }
}
