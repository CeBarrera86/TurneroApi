namespace TurneroApi.DTOs;

public class ContenidoDto
{
  public uint Id { get; set; }
  public string Nombre { get; set; } = string.Empty;
  public string Tipo { get; set; } = string.Empty;
  public bool Activa { get; set; }
  public DateTime Fecha { get; set; }
  public string UrlMiniatura { get; set; } = string.Empty;
  public string UrlArchivo { get; set; } = string.Empty;
}
