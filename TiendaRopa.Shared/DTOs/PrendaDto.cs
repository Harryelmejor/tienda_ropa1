namespace TiendaRopa.Shared.DTOs;

public class PrendaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? Talla { get; set; }
    public string? Color { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
}

public class PrendaCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? Talla { get; set; }
    public string? Color { get; set; }
    public int CategoriaId { get; set; }
}
