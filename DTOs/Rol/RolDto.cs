using System;

namespace TurneroApi.DTOs.Rol;

public class RolDto
{
  public int Id { get; set; }
  public string Nombre { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}