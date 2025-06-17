using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces;

namespace TurneroApi.Services
{
    public class GeaSeguridadService : IGeaSeguridadService
    {
        private readonly GeaSeguridadDbContext _context;

        public GeaSeguridadService(GeaSeguridadDbContext context)
        {
            _context = context;
        }

        public async Task<GeaSeguridad?> ObtenerUsuarioAsync(string username)
        {
            try
            {
                return await _context.GeaSeguridad.FirstOrDefaultAsync(u => u.USU_CODIGO == username);
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