namespace TurneroApi.DTOs;

public class UserDto
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public ulong Role { get; set; }
    public string Username { get; set; } = null!;

    // Nombres representativos opcionales para mostrar en UI
    public string? RolTipo { get; set; }
}
