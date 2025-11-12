using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Contenido;

public class ContenidoCrearDto
{
  [Required]
  public IFormFile Archivo { get; set; } = null!;
  public bool Activo { get; set; } = true;
}
