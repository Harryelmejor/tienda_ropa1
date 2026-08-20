using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    public ICollection<Prenda> Prendas { get; set; } = new List<Prenda>();
}
