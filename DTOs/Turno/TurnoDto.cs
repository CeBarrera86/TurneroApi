namespace TurneroApi.DTOs.Turno;

using System;
using TurneroApi.DTOs.Puesto;
using TurneroApi.DTOs.Ticket;
using TurneroApi.DTOs.Estado;

public class TurnoDto
{
  public ulong Id { get; set; }
  public int PuestoId { get; set; }
  public ulong TicketId { get; set; }
  public DateTime FechaInicio { get; set; }
  public DateTime? FechaFin { get; set; }
  public int EstadoId { get; set; }
  public PuestoDto PuestoNavigation { get; set; } = null!;
  public TicketDto TicketNavigation { get; set; } = null!;
  public EstadoDto EstadoNavigation { get; set; } = null!;
}