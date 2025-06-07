namespace TurneroApi.DTOs;

public class HistorialDto
{
    public ulong Id { get; set; }
    public ulong Turno { get; set; }
    public ulong Puesto { get; set; }
    public ulong Estado { get; set; }
    public ulong? DerPara { get; set; }
    public DateTime? CreatedAt { get; set; }
}
