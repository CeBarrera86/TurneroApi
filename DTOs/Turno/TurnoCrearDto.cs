using System.ComponentModel.DataAnnotations;
using System;

namespace TurneroApi.DTOs.Turno;

public class TurnoCrearDto
{
  [Required(ErrorMessage = "El puesto es obligatorio.")]
  public int PuestoId { get; set; }

  [Required(ErrorMessage = "El ticket es obligatorio.")]
  public ulong TicketId { get; set; }

  [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
  public DateTime FechaInicio { get; set; }

  [Required(ErrorMessage = "El estado es obligatorio.")]
  public int EstadoId { get; set; }
}