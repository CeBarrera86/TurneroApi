namespace TurneroApi.Models;

public partial class MostradorSector
{
  public int MostradorId { get; set; }
  public int SectorId { get; set; }

  public virtual Mostrador Mostrador { get; set; } = null!;
  public virtual Sector Sector { get; set; } = null!;
}