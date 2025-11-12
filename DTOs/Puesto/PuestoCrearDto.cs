using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Puesto;

public class PuestoCrearDto
{
  [Required(ErrorMessage = "El usuario es obligatorio.")]
  public int UsuarioId { get; set; }

  [Required(ErrorMessage = "El mostrador es obligatorio.")]
  public int MostradorId { get; set; }

  public bool Activo { get; set; } = true;
}