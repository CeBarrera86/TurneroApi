namespace TurneroApi.DTOs;

public class TicketDto
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public uint Numero { get; set; }
    public ulong ClienteId { get; set; }
    public DateTime Fecha { get; set; }
    public uint SectorIdOrigen { get; set; }
    public uint SectorIdActual { get; set; }
    public uint EstadoId { get; set; }
    public DateTime Actualizado { get; set; }

    // Nombres representativos opcionales para mostrar en UI
    public string? ClienteNombre { get; set; }
    public string? SectorOrigenNombre { get; set; }
    public string? SectorActualNombre { get; set; }
    public string? EstadoNombre { get; set; }
}
