namespace TurneroApi.DTOs.Usuario;

using System;

public class UsuarioDto
{
  public int Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string Apellido { get; set; } = null!;
  public string Username { get; set; } = null!;
  public bool Activo { get; set; }
  public string RolNombre { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}