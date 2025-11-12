using System;

namespace TurneroApi.DTOs.Permiso;

public class PermisoDto
{
  public int Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string? Descripcion { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
