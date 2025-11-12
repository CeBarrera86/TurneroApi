using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Sector;

public class SectorCrearDto
{
  public int? PadreId { get; set; }

  [StringLength(3, ErrorMessage = "La letra no puede exceder los 3 caracteres.")]
  [RegularExpression("^[A-Z]{1,3}$", ErrorMessage = "La letra debe contener solo letras mayúsculas.")]
  public string? Letra { get; set; }

  [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
  public string? Nombre { get; set; }

  [StringLength(120, ErrorMessage = "La descripción no puede exceder los 120 caracteres.")]
  public string? Descripcion { get; set; }

  public bool Activo { get; set; } = true;
}