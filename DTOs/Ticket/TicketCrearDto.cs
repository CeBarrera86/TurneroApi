using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Ticket;

public class TicketCrearDto
{
  [StringLength(2)]
  [RegularExpression("^[A-Z]{1,2}$", ErrorMessage = "La letra debe contener solo letras mayúsculas.")]
  public string? Letra { get; set; }

  [Required(ErrorMessage = "El cliente es obligatorio.")]
  public ulong ClienteId { get; set; }

  [Required(ErrorMessage = "El sector de origen es obligatorio.")]
  public int SectorIdOrigen { get; set; }
}