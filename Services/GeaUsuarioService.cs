using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces;

namespace TurneroApi.Services
{
    public class GeaUsuarioService : IGeaUsuarioService
    {
        private readonly GeaSeguridadDbContext _context;
        private readonly string _modoGea;


        public GeaUsuarioService(GeaSeguridadDbContext context, IConfiguration configuration)
        {
            _context = context;
            _modoGea = configuration["GeaSettings:Modo"]
                        ?? throw new InvalidOperationException("La configuración 'GeaSettings:Modo' no está definida.");
        }

        public async Task<GeaUsuario?> ObtenerUsuarioAsync(string username)
        {
            return await _context.GeaUsuarios.FirstOrDefaultAsync(u => u.USU_CODIGO == username);
        }

        public bool ValidarPassword(string inputPassword, string encryptedPassword)
        {
            if (_modoGea == "Real")
            {
                return inputPassword == Hasher.Decod(encryptedPassword);
            }

            // Modo Mock: comparar en texto plano
            return inputPassword == encryptedPassword;
        }

    }
}
