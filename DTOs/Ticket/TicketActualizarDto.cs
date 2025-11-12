using System.ComponentModel.DataAnnotations;
using System;

namespace TurneroApi.DTOs.Ticket;

public class TicketActualizarDto
{
  public int? SectorIdActual { get; set; }
  public int? EstadoId { get; set; }
  public DateTime? Actualizado { get; set; }
}