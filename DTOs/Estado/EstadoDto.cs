namespace TurneroApi.DTOs.Estado;

public class EstadoDto
{
  public int Id { get; set; }
  public string Letra { get; set; } = null!;
  public string Descripcion { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}