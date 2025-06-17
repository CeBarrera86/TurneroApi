using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces;

namespace TurneroApi.Services
{
    public class GeaUsuarioService : IGeaUsuarioService
    {
        private readonly GeaSeguridadDbContext _context;

        public GeaUsuarioService(GeaSeguridadDbContext context)
        {
            _context = context;
        }

        public async Task<GeaUsuario?> ObtenerUsuarioAsync(string username)
        {
            try
            {
                return await _context.GeaUsuarios.FirstOrDefaultAsync(u => u.USU_CODIGO == username);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ValidarPassword(string inputPassword, string encryptedPassword)
        {
            return inputPassword == Hasher.Decod(encryptedPassword);
        }
    }
}