namespace TurneroApi.DTOs.Historial;

using System;
using TurneroApi.DTOs.Estado;
using TurneroApi.DTOs.Puesto;
using TurneroApi.DTOs.Ticket;
using TurneroApi.DTOs.Turno;
using TurneroApi.DTOs.Usuario;

public class HistorialDto
{
  public ulong Id { get; set; }  public ulong TicketId { get; set; }
  public int EstadoId { get; set; }
  public DateTime Fecha { get; set; }
  public int? PuestoId { get; set; }
  public ulong? TurnoId { get; set; }
  public int? UsuarioId { get; set; }
  public string? Comentarios { get; set; }

  public TicketDto TicketNavigation { get; set; } = null!;
  public EstadoDto EstadoNavigation { get; set; } = null!;
  public PuestoDto? PuestoNavigation { get; set; }
  public TurnoDto? TurnoNavigation { get; set; }
  public UsuarioDto? UsuarioNavigation { get; set; }
}