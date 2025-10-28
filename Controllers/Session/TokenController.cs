using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurneroApi.Data;
using TurneroApi.Models.Session;
using TurneroApi.Interfaces.GeaPico;

namespace TurneroApi.Controllers
{
  [ApiController]
  [Route("api/token")]
  public class TokenController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly TurneroDbContext _turneroContext;
    private readonly ILogger<TokenController> _logger;
    private readonly IGeaSeguridadService _geaSeguridadService;

    public TokenController(IConfiguration config, TurneroDbContext turneroContext, ILogger<TokenController> logger, IGeaSeguridadService geaSeguridadService)
    {
      _config = config;
      _turneroContext = turneroContext;
      _logger = logger;
      _geaSeguridadService = geaSeguridadService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request)
    {
      var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

      if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        return BadRequest("Credenciales inválidas.");

      var geaUsuario = await _geaSeguridadService.ObtenerUsuarioAsync(request.Username);
      if (geaUsuario == null)
        return Unauthorized("Usuario no encontrado.");

      if (!_geaSeguridadService.ValidarPassword(request.Password, geaUsuario.USU_PASSWORD))
        return Unauthorized("Contraseña incorrecta.");

      var mostrador = await _turneroContext.Mostradores.FirstOrDefaultAsync(m => m.Ip == clientIp);
      if (mostrador == null)
        return Unauthorized("IP del mostrador no registrada o inválida.");

      var turneroUser = await _turneroContext.Usuarios.Include(u => u.RolNavigation).FirstOrDefaultAsync(u => u.Username == request.Username);
      if (turneroUser == null)
        return Unauthorized("Usuario no configurado en el sistema de turnos.");

      var claims = new List<Claim>
      {
          new Claim(ClaimTypes.NameIdentifier, turneroUser.Id.ToString()),
          new Claim(ClaimTypes.Name, turneroUser.Username),
          new Claim(ClaimTypes.Role, turneroUser.RolNavigation.Tipo)
      };

      var jwtKey = _config["Jwt:Key"];
      var jwtIssuer = _config["Jwt:Issuer"];
      var jwtAudience = _config["Jwt:Audience"];

      if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
        return StatusCode(500, "Configuración JWT incompleta en appsettings.json.");

      if (!int.TryParse(_config["Jwt:DurationInMinutes"], out int duration))
        duration = 60;

      try
      {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(duration),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var responsePayload = new
        {
          token = tokenString,
          username = turneroUser.Username,
          name = turneroUser.Nombre,
          rol = turneroUser.RolNavigation.Tipo,
          mostradorTipo = mostrador.Tipo,
          mostradorSector = mostrador.SectorId
        };

        _logger.LogInformation("Login exitoso. Respuesta generada: {@Response}", responsePayload);

        return Ok(responsePayload);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al generar el token o construir la respuesta.");
        return StatusCode(500, "Error interno al generar el token.");
      }
    }
  }
}