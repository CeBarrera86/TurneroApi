using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class ContenidoCrearDto
{
  [Required]
  public IFormFile Archivo { get; set; } = null!;
  public bool Activa { get; set; } = true;
}
