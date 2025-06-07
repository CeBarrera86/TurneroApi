namespace TurneroApi.DTOs;

public class SectorDto
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}
