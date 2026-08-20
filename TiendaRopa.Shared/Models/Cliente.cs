using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Teléfono no válido")]
    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Email no válido")]
    [StringLength(150)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [StringLength(250)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Display(Name = "Nombre completo")]
    public string NombreCompleto => $"{Nombre} {Apellido}";

    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
