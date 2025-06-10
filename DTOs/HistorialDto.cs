namespace TurneroApi.DTOs;

public class HistorialDto
{
    public ulong Id { get; set; }
    public ulong TicketId { get; set; }
    public uint EstadoId { get; set; }
    public DateTime Fecha { get; set; }
    public uint? PuestoId { get; set; }
    public ulong? TurnoId { get; set; }
    public uint? UsuarioId { get; set; }
    public string? Comentarios { get; set; }
    public DateTime? CreatedAt { get; set; }
}
