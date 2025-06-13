using TurneroApi.Models.GeaPico;

namespace TurneroApi.Interfaces
{
    public interface IGeaUsuarioService
    {
        Task<GeaUsuario?> ObtenerUsuarioAsync(string username);
        bool ValidarPassword(string inputPassword, string encryptedPassword);
    }
}
