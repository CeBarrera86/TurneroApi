using System;
using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Historial;

public class HistorialCrearDto
{
  [Required(ErrorMessage = "El ticket es obligatorio.")]
  public ulong TicketId { get; set; }

  [Required(ErrorMessage = "El estado es obligatorio.")]
  public int EstadoId { get; set; }

  [Required(ErrorMessage = "La fecha es obligatoria.")]
  public DateTime Fecha { get; set; }

  public int? PuestoId { get; set; }
  public ulong? TurnoId { get; set; }
  public int? UsuarioId { get; set; }

  [StringLength(255, ErrorMessage = "Los comentarios no pueden exceder los 255 caracteres.")]
  public string? Comentarios { get; set; }
}