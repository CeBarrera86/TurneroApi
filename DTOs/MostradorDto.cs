namespace TurneroApi.DTOs;

public class MostradorDto
{
    public ulong Id { get; set; }
    public uint Numero { get; set; }
    public string Ip { get; set; } = null!;
    public string? Alfa { get; set; }
    public string? Tipo { get; set; }
    public ulong Sector { get; set; }
}
