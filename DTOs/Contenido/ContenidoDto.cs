using System;

namespace TurneroApi.DTOs.Contenido;

public class ContenidoDto
{
  public uint Id { get; set; }
  public string Nombre { get; set; } = string.Empty;
  public string Tipo { get; set; } = string.Empty;
  public bool Activo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public string UrlMiniatura { get; set; } = string.Empty;
  public string UrlArchivo { get; set; } = string.Empty;
}
