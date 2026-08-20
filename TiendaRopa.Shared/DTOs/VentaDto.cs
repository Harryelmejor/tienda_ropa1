namespace TiendaRopa.Shared.DTOs;

public class VentaDto
{
    public int Id { get; set; }
    public string NumeroFactura => $"VTA-{Id:D5}";
    public int? ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int EmpleadoId { get; set; }
    public string? EmpleadoNombre { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}

public class DetalleVentaDto
{
    public int Id { get; set; }
    public int PrendaId { get; set; }
    public string? PrendaNombre { get; set; }
    public string? PrendaTalla { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}

public class VentaCreateDto
{
    public int? ClienteId { get; set; }
    public int EmpleadoId { get; set; }
    public List<DetalleVentaCreateDto> Items { get; set; } = new();
}

public class DetalleVentaCreateDto
{
    public int PrendaId { get; set; }
    public int Cantidad { get; set; } = 1;
}
