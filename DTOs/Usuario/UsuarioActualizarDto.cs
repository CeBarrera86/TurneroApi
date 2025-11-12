using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Usuario;

public class UsuarioActualizarDto
{
  [StringLength(50)]
  public string? Nombre { get; set; }

  [StringLength(50)]
  public string? Apellido { get; set; }

  [StringLength(30)]
  public string? Username { get; set; }

  public int? RolId { get; set; }
  public bool? Activo { get; set; }
}