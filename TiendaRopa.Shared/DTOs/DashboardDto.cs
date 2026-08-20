namespace TiendaRopa.Shared.DTOs;

public class DashboardDto
{
    public int TotalPrendas { get; set; }
    public int TotalClientes { get; set; }
    public int TotalEmpleados { get; set; }
    public int TotalVentas { get; set; }
    public int InventarioTotal { get; set; }
    public decimal VentasMes { get; set; }
    public List<VentaDto> VentasRecientes { get; set; } = new();
    public List<PrendaDto> StockBajo { get; set; } = new();
}
