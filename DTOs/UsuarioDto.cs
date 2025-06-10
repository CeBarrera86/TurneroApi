namespace TurneroApi.DTOs;

public class UsuarioDto
{
    public uint Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Username { get; set; } = null!;
    public uint RolId { get; set; }

    // Nombres representativos opcionales para mostrar en UI
    public string? RolTipo { get; set; }
}
