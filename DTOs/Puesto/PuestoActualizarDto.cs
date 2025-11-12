namespace TurneroApi.DTOs.Puesto;

public class PuestoActualizarDto
{
  public DateTime? Login { get; set; }
  public DateTime? Logout { get; set; }
  public bool Activo { get; set; }
}