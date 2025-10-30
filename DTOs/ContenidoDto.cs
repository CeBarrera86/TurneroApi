namespace TurneroApi.DTOs;

public class ContenidoDto
{
  public uint Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string Ruta { get; set; } = null!;
  public string Tipo { get; set; } = null!;
  public bool Activa { get; set; }
  public DateTime Fecha { get; set; }
}
