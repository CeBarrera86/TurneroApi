using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Permiso;

public class PermisoCrearDto
{
  [Required(ErrorMessage = "El nombre del permiso es obligatorio.")]
  [StringLength(50, ErrorMessage = "El nombre del permiso no puede exceder los 50 caracteres.")]
  public string Nombre { get; set; } = null!;

  [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
  public string? Descripcion { get; set; }
}
