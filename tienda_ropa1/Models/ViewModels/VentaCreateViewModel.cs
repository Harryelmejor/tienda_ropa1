using System.ComponentModel.DataAnnotations;

namespace tienda_ropa1.Models.ViewModels;

public class VentaCreateViewModel
{
    [Display(Name = "Cliente")]
    public int? ClienteId { get; set; }

    [Required(ErrorMessage = "Seleccione un empleado")]
    [Display(Name = "Empleado")]
    public int EmpleadoId { get; set; }

    public List<DetalleVentaItem> Items { get; set; } = new() { new DetalleVentaItem() };
}

public class DetalleVentaItem
{
    [Required(ErrorMessage = "Seleccione una prenda")]
    [Display(Name = "Prenda")]
    public int PrendaId { get; set; }

    [Required]
    [Range(1, 9999, ErrorMessage = "La cantidad debe ser al menos 1")]
    [Display(Name = "Cantidad")]
    public int Cantidad { get; set; } = 1;
}
