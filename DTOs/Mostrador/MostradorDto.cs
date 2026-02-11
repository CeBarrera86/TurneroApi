using System;
using System.Collections.Generic;
using TurneroApi.DTOs.Sector;

namespace TurneroApi.DTOs.Mostrador;

public class MostradorDto
{
  public int Id { get; set; }
  public int Numero { get; set; }
  public string Ip { get; set; } = null!;
  public string? Tipo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public List<SectorDto> Sectores { get; set; } = new();
}
