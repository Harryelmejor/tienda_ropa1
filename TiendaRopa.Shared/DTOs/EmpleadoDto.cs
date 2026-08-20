namespace TiendaRopa.Shared.DTOs;

public class EmpleadoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Cargo { get; set; }
    public int CantidadVentas { get; set; }
}

public class EmpleadoCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Cargo { get; set; }
}
