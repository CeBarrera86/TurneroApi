using TurneroApi.Models.GeaPico;

namespace TurneroApi.Interfaces.GeaPico
{
    public interface IGeaSeguridadService
    {
        Task<GeaSeguridad?> ObtenerUsuarioAsync(string username);
        bool ValidarPassword(string inputPassword, string encryptedPassword);
    }
}
