using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Sector;

public class SectorActualizarDto
{
  public int? PadreId { get; set; }

  [StringLength(3)]
  [RegularExpression("^[A-Z]{1,3}$", ErrorMessage = "La letra debe contener solo letras mayúsculas.")]
  public string? Letra { get; set; }

  [StringLength(50)]
  public string? Nombre { get; set; }

  [StringLength(120)]
  public string? Descripcion { get; set; }

  public bool? Activo { get; set; }
}
