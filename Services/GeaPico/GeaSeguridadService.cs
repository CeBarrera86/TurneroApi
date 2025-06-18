using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces.GeaPico;

namespace TurneroApi.Services.GeaPico
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
            return inputPassword == Session.Hasher.Decod(encryptedPassword);
        }
    }
}