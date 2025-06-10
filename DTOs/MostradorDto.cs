namespace TurneroApi.DTOs;

public class MostradorDto
{
    public uint Id { get; set; }
    public uint Numero { get; set; }
    public string Ip { get; set; } = null!;
    public string? Tipo { get; set; }
    public uint SectorId { get; set; }
    public string? SectorNombre { get; set; }
}
