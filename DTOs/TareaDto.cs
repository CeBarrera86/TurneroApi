namespace TurneroApi.DTOs;

public class TareaDto
{
    public ulong Id { get; set; }
    public ulong Sector { get; set; }
    public string Descripcion { get; set; } = null!;
}
