namespace TurneroApi.DTOs.Puesto;

public class PuestoDto
{
  public int Id { get; set; }
  public int UsuarioId { get; set; }
  public int MostradorId { get; set; }
  public DateTime? Login { get; set; }
  public DateTime? Logout { get; set; }
  public bool Activo { get; set; }

  // Campo adicional para mostrar el nombre completo del usuario
  public string UsuarioNombre { get; set; } = string.Empty;

  // Campo adicional para mostrar la IP del mostrador
  public string MostradorIp { get; set; } = string.Empty;
}