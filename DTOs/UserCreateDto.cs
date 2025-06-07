namespace TurneroApi.DTOs;

public class UserCreateDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public ulong Role { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
