namespace TurneroApi.DTOs;

public class PuestoDto
{
    public ulong Id { get; set; }
    public ulong Mostrador { get; set; }
    public ulong User { get; set; }
    public DateTime? Login { get; set; }
    public DateTime? Logout { get; set; }
}
