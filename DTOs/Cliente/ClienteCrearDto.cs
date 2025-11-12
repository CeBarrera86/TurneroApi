using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Cliente;

public class ClienteCrearDto
{
  [Required(ErrorMessage = "El DNI es obligatorio.")]
  [RegularExpression("^[0-9]+$", ErrorMessage = "El DNI debe contener solo números.")]
  [StringLength(10, ErrorMessage = "El DNI no puede exceder los 10 caracteres.")]
  public string Dni { get; set; } = null!;

  [Required(ErrorMessage = "El titular es obligatorio.")]
  [StringLength(50, ErrorMessage = "El titular no puede exceder los 50 caracteres.")]
  public string Titular { get; set; } = null!;
}