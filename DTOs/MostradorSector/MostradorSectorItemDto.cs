namespace TurneroApi.DTOs.MostradorSector;

public class MostradorSectorItemDto
{
    public int MostradorId { get; set; }
    public int SectorId { get; set; }

    public string? MostradorIp { get; set; }
    public int? MostradorNumero { get; set; }

    public string? SectorNombre { get; set; }
    public string? SectorLetra { get; set; }
}