namespace TurneroApi.Models.Session;

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ClientIp { get; set; } = null!;
}