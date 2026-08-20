using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaRopa.Shared.Models;

public class Prenda
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Precio")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio")]
    [Range(0, 99999, ErrorMessage = "El stock no puede ser negativo")]
    [Display(Name = "Stock (Inventario)")]
    public int Stock { get; set; }

    [StringLength(20)]
    [Display(Name = "Talla")]
    public string? Talla { get; set; }

    [StringLength(50)]
    [Display(Name = "Color")]
    public string? Color { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
}
