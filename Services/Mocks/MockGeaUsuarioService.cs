using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces;

namespace TurneroApi.Services.Mocks
{
    public class MockGeaUsuarioService : IGeaUsuarioService
    {
        public Task<GeaUsuario?> ObtenerUsuarioAsync(string username)
        {
            if (username != "cbarrera")
                return Task.FromResult<GeaUsuario?>(null);

            return Task.FromResult<GeaUsuario?>(new GeaUsuario
            {
                USU_CODIGO = "cbarrera",
                USU_PASSWORD = Hasher.Cod("cesar2025")
            });
        }

        public bool ValidarPassword(string inputPassword, string encryptedPassword)
        {
            return inputPassword == Hasher.Decod(encryptedPassword);
        }
    }
}
