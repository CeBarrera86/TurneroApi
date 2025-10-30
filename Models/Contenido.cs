namespace TurneroApi.Models;

public class Contenido
{
  public uint Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string Ruta { get; set; } = null!;
  public string Tipo { get; set; } = null!; // "imagen" o "video"
  public bool Activa { get; set; } = true;
  public DateTime Fecha { get; set; } = DateTime.Now;
}
