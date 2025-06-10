namespace TurneroApi.DTOs;

public class TurnoDto
{
    public ulong Id { get; set; }
    public uint PuestoId { get; set; }
    public ulong TicketId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public uint EstadoId { get; set; }
}
