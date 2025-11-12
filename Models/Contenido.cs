namespace TurneroApi.Models;

public partial class Contenido
{
  public uint Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string Ruta { get; set; } = null!;
  public string Tipo { get; set; } = null!; // "imagen" o "video"
  public bool Activo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}