using TurneroApi.Models.GeaPico;

namespace TurneroApi.Interfaces
{
    public interface IGeaSeguridadService
    {
        Task<GeaSeguridad?> ObtenerUsuarioAsync(string username);
        bool ValidarPassword(string inputPassword, string encryptedPassword);
    }
}
