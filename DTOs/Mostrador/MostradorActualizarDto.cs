using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Mostrador;

public class MostradorActualizarDto
{
  public int? Numero { get; set; }

  [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
      ErrorMessage = "El formato de la dirección IP no es válido. Debe ser IPv4 (ej. 192.168.1.1).")]
  public string? Ip { get; set; }

  [StringLength(10)]
  public string? Tipo { get; set; }
}
