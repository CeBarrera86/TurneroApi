using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurneroApi.Data;
using TurneroApi.Models.Session;
using TurneroApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace TurneroApi.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly TurneroDbContext _turneroContext;
        private readonly ILogger<TokenController> _logger;
        private readonly IGeaUsuarioService _geaUsuarioService;

        public TokenController(
            IConfiguration config,
            TurneroDbContext turneroContext,
            ILogger<TokenController> logger,
            IGeaUsuarioService geaUsuarioService)
        {
            _config = config;
            _turneroContext = turneroContext;
            _logger = logger;
            _geaUsuarioService = geaUsuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Intento de login recibido. Usuario: {Username}, IP Cliente: {ClientIp}",
                request.Username,
                request.ClientIp);

            // 0. Validación básica
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Credenciales inválidas.");

            // 1. Obtener usuario desde GeaSeguridad (real o mock)
            var geaUsuario = await _geaUsuarioService.ObtenerUsuarioAsync(request.Username);
            if (geaUsuario == null)
                return Unauthorized("Usuario no encontrado.");

            // 2. Validar contraseña
            if (!_geaUsuarioService.ValidarPassword(request.Password, geaUsuario.USU_PASSWORD))
                return Unauthorized("Contraseña incorrecta.");

            // 3. Buscar mostrador por IP
            var mostrador = await _turneroContext.Mostradores.FirstOrDefaultAsync(m => m.Ip == request.ClientIp);
            if (mostrador == null)
                return Unauthorized("IP del mostrador no registrada o inválida.");

            // 4. Obtener usuario en Turnero
            var turneroUser = await _turneroContext.Usuarios
                .Include(u => u.RolNavigation)
                .FirstOrDefaultAsync(u => u.Username == request.Username);
            if (turneroUser == null)
                return Unauthorized("Usuario no configurado en el sistema de turnos.");

            // 5. Crear claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, turneroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, turneroUser.Username),
                new Claim(ClaimTypes.Role, turneroUser.RolNavigation.Tipo)
            };

            // 6. Validar configuración JWT
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
                return StatusCode(500, "Configuración JWT incompleta en appsettings.json.");

            if (!int.TryParse(_config["Jwt:DurationInMinutes"], out int duration))
                duration = 60;

            // 7. Generar token
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

            return Ok(new
            {
                token = tokenString,
                username = turneroUser.Username,
                name = turneroUser.Nombre,
                rol = turneroUser.RolNavigation.Tipo,
                mostradorTipo = mostrador.Tipo,
                mostradorSector = mostrador.SectorId
            });
        }
    }
}