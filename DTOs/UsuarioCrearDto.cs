
namespace TurneroApi.DTOs;

public class UsuarioCrearDto
{
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Username { get; set; } = null!;
    public uint RolId { get; set; }
}
