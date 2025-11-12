using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.RolPermiso;

public class RolPermisoCrearDto
{
  [Required]
  public int RolId { get; set; }

  [Required]
  public int PermisoId { get; set; }
}