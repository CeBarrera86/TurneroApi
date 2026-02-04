namespace TurneroApi.DTOs.Sector;

using System;

public class SectorDto
{
  public int Id { get; set; }
  public int? PadreId { get; set; }
  public string? Letra { get; set; }
  public string? Nombre { get; set; }
  public string? Descripcion { get; set; }
  public bool Activo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
