namespace TurneroApi.DTOs;

public class TicketDto
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public uint Numero { get; set; }
    public DateOnly FechaTicket { get; set; }
    public ulong Cliente { get; set; }
    public ulong Sector { get; set; }
    public ulong Estado { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Nombres representativos opcionales para mostrar en UI
    public string? ClienteNombre { get; set; }
    public string? SectorNombre { get; set; }
    public string? EstadoNombre { get; set; }
}
