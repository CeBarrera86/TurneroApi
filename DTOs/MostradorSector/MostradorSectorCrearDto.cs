using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.MostradorSector;

public class MostradorSectorCrearDto
{
  [Required]
  public int MostradorId { get; set; }

  [Required]
  public int SectorId { get; set; }
}