using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurneroApi.Data;
using TurneroApi.Models.Session;
using TurneroApi.Services;
using System.Linq;

namespace TurneroApi.Controllers;

[ApiController]
[Route("api/token")] // Es buena práctica usar "api/" en el prefijo de ruta para APIs
public class TokenController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly TurneroDbContext _turneroContext;
    private readonly GeaSeguridadDbContext _geaSeguridadContext;

    public TokenController(IConfiguration config, TurneroDbContext turneroContext, GeaSeguridadDbContext geaSeguridadContext)
    {
        _config = config;
        _turneroContext = turneroContext;
        _geaSeguridadContext = geaSeguridadContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> GenerateToken([FromQuery] LoginRequest request)
    {
        // 1. Validar existencia del usuario en GeaSeguridad_Corpico
        var geaUsuario = await _geaSeguridadContext.GeaUsuarios.FirstOrDefaultAsync(u => u.USU_CODIGO == request.Username);
        if (geaUsuario == null)
            return Unauthorized("Usuario no encontrado.");

        // 2. Desencriptar la contraseña almacenada y compararla
        string decryptedDbPassword = Hasher.Decod(geaUsuario.USU_PASSWORD);
        if (request.Password != decryptedDbPassword)
            return Unauthorized("Contraseña incorrecta.");

        // 3. Buscar el mostrador por la IP proporcionada por el cliente
        var mostrador = await _turneroContext.Mostradores.FirstOrDefaultAsync(m => m.Ip == request.ClientIp);
        if (mostrador == null)
        {
            return Unauthorized("IP del mostrador no registrada o inválida.");
        }

        // 4. Obtener los detalles del usuario de la base de datos de Turnero
        var turneroUser = await _turneroContext.Users.Include(u => u.RoleNavigation)
                        .FirstOrDefaultAsync(u => u.Username == request.Username);
        if (turneroUser == null)
        {
            return Unauthorized("Usuario no configurado en el sistema de turnos.");
        }

        // 5. Construcción de claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, turneroUser.Id.ToString()), // Id del usuario de Turnero
            new Claim(ClaimTypes.Name, turneroUser.Username),                // username
            new Claim(ClaimTypes.Role, turneroUser.RoleNavigation.Tipo)      // rol (ej. "Administrador", "Operador")
        };

        // 5. Validar configuración JWT
        var jwtKey = _config["Jwt:Key"];
        var jwtIssuer = _config["Jwt:Issuer"];
        var jwtAudience = _config["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
            return StatusCode(500, "Configuración JWT incompleta en appsettings.json.");

        // 6. Generar token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"] ?? "60")), // Duración configurable
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            username = turneroUser.Username,
            name = turneroUser.Name,
            rol = turneroUser.RoleNavigation.Tipo,
            mostradorTipo = mostrador.Tipo,
            mostradorSector = mostrador.Sector
        });
    }
}