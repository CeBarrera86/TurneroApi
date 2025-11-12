namespace TurneroApi.DTOs.Cliente;

public class ClienteDto
{
  public ulong Id { get; set; }
  public string Dni { get; set; } = null!;
  public string Titular { get; set; } = null!;
}
