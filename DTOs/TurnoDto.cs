namespace TurneroApi.DTOs;

public class TurnoDto
{
    public ulong Id { get; set; }
    public ulong Ticket { get; set; }
    public ulong Puesto { get; set; }
    public DateTime? CreatedAt { get; set; }
}
