using TurneroApi.Models.GeaPico;
using TurneroApi.Interfaces;

namespace TurneroApi.Services.Mocks
{
    public class MockGeaSeguridadService : IGeaSeguridadService
    {
        public Task<GeaSeguridad?> ObtenerUsuarioAsync(string username)
        {
            if (username == "cbarrera")
            {
                return Task.FromResult<GeaSeguridad?>(new GeaSeguridad
                {
                    USU_CODIGO = "cbarrera",
                    USU_PASSWORD = Hasher.Cod("cesar2025")
                });
            }
            // Agrega usuario de prueba
            // else if (username == "ncarracedo")
            // {
            //      return Task.FromResult<GeaUsuario?>(new GeaUsuario
            //     {
            //         USU_CODIGO = "ncarracedo",
            //         USU_PASSWORD = Hasher.Cod("noe25")
            //     });
            // }
            else
            {
                return Task.FromResult<GeaSeguridad?>(null);
            }
        }

        public bool ValidarPassword(string inputPassword, string encryptedPassword)
        {
            return inputPassword == Hasher.Decod(encryptedPassword);
        }
    }
}