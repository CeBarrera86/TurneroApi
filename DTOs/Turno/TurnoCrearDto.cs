using System.ComponentModel.DataAnnotations;
using System;

namespace TurneroApi.DTOs.Turno;

public class TurnoCrearDto
{
  [Required(ErrorMessage = "El ticket es obligatorio.")]
  public ulong TicketId { get; set; }
}