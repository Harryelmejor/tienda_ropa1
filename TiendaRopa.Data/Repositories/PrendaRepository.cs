using Microsoft.EntityFrameworkCore;
using TiendaRopa.Data;
using TiendaRopa.Shared.Models;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data.Repositories;

public class PrendaRepository : IPrendaService
{
    private readonly TiendaDbContext _context;

    public PrendaRepository(TiendaDbContext context) => _context = context;

    public async Task<List<PrendaDto>> GetAllAsync(string? buscar = null, int? categoriaId = null)
    {
        var query = _context.Prendas.Include(p => p.Categoria).AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            query = query.Where(p => p.Nombre.Contains(buscar) || (p.Talla != null && p.Talla.Contains(buscar)));
        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId);

        return await query.OrderBy(p => p.Nombre)
            .Select(p => new PrendaDto
            {
                Id = p.Id, Nombre = p.Nombre, Descripcion = p.Descripcion,
                Precio = p.Precio, Stock = p.Stock, Talla = p.Talla,
                Color = p.Color, CategoriaId = p.CategoriaId, CategoriaNombre = p.Categoria!.Nombre
            })
            .ToListAsync();
    }

    public async Task<PrendaDto?> GetByIdAsync(int id)
    {
        return await _context.Prendas
            .Include(p => p.Categoria)
            .Where(p => p.Id == id)
            .Select(p => new PrendaDto
            {
                Id = p.Id, Nombre = p.Nombre, Descripcion = p.Descripcion,
                Precio = p.Precio, Stock = p.Stock, Talla = p.Talla,
                Color = p.Color, CategoriaId = p.CategoriaId, CategoriaNombre = p.Categoria!.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PrendaDto> CreateAsync(PrendaCreateDto dto)
    {
        var prenda = new Prenda
        {
            Nombre = dto.Nombre, Descripcion = dto.Descripcion, Precio = dto.Precio,
            Stock = dto.Stock, Talla = dto.Talla, Color = dto.Color, CategoriaId = dto.CategoriaId
        };
        _context.Prendas.Add(prenda);
        await _context.SaveChangesAsync();
        var cat = await _context.Categorias.FindAsync(prenda.CategoriaId);
        return new PrendaDto
        {
            Id = prenda.Id, Nombre = prenda.Nombre, Descripcion = prenda.Descripcion,
            Precio = prenda.Precio, Stock = prenda.Stock, Talla = prenda.Talla,
            Color = prenda.Color, CategoriaId = prenda.CategoriaId, CategoriaNombre = cat?.Nombre
        };
    }

    public async Task<PrendaDto?> UpdateAsync(int id, PrendaCreateDto dto)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null) return null;
        prenda.Nombre = dto.Nombre;
        prenda.Descripcion = dto.Descripcion;
        prenda.Precio = dto.Precio;
        prenda.Stock = dto.Stock;
        prenda.Talla = dto.Talla;
        prenda.Color = dto.Color;
        prenda.CategoriaId = dto.CategoriaId;
        await _context.SaveChangesAsync();
        var cat = await _context.Categorias.FindAsync(prenda.CategoriaId);
        return new PrendaDto
        {
            Id = prenda.Id, Nombre = prenda.Nombre, Descripcion = prenda.Descripcion,
            Precio = prenda.Precio, Stock = prenda.Stock, Talla = prenda.Talla,
            Color = prenda.Color, CategoriaId = prenda.CategoriaId, CategoriaNombre = cat?.Nombre
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null) return false;
        if (await _context.DetalleVentas.AnyAsync(d => d.PrendaId == id))
            return false;
        _context.Prendas.Remove(prenda);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PrendaDto>> GetStockBajoAsync(int umbral = 10)
    {
        return await _context.Prendas
            .Include(p => p.Categoria)
            .Where(p => p.Stock <= umbral)
            .OrderBy(p => p.Stock)
            .Select(p => new PrendaDto
            {
                Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Stock = p.Stock,
                Talla = p.Talla, Color = p.Color, CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria!.Nombre
            })
            .ToListAsync();
    }
}
