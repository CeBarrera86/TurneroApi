namespace TurneroApi.DTOs;

public class EstadoDto
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
}
