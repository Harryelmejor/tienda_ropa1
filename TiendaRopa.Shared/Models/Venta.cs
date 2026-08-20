using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaRopa.Shared.Models;

public class Venta
{
    public int Id { get; set; }

    [Display(Name = "Cliente")]
    public int? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [Required(ErrorMessage = "El empleado es obligatorio")]
    [Display(Name = "Empleado")]
    public int EmpleadoId { get; set; }

    public Empleado? Empleado { get; set; }

    [Required]
    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Total")]
    public decimal Total { get; set; }

    [Display(Name = "N° Factura")]
    public string NumeroFactura => $"VTA-{Id:D5}";

    public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
}

public class DetalleVenta
{
    public int Id { get; set; }

    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    [Required]
    [Display(Name = "Prenda")]
    public int PrendaId { get; set; }

    public Prenda? Prenda { get; set; }

    [Required]
    [Range(1, 9999, ErrorMessage = "La cantidad debe ser al menos 1")]
    [Display(Name = "Cantidad")]
    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Precio unitario")]
    public decimal PrecioUnitario { get; set; }

    [Display(Name = "Subtotal")]
    public decimal Subtotal => Cantidad * PrecioUnitario;
}
