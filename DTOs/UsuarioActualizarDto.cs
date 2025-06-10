
namespace TurneroApi.DTOs;

public class UsuarioActualizarDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public uint RolId { get; set; }
}
