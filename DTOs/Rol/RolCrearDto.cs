using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Rol;

public class RolCrearDto
{
  [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
  [StringLength(20, ErrorMessage = "El nombre del rol no puede exceder los 20 caracteres.")]
  public string Nombre { get; set; } = null!;
}
