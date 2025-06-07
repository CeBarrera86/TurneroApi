namespace TurneroApi.DTOs;

public class RolDto
{
    public ulong Id { get; set; }
    public string Tipo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
}
