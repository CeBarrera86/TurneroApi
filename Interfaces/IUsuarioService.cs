using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface IUsuarioService
  {
    Task<IEnumerable<Usuario>> GetUsuariosAsync();
    Task<Usuario?> GetUsuarioAsync(uint id);
    Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario);
    Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(uint id, UsuarioActualizarDto usuarioActualizarDto);
    Task<bool> DeleteUsuarioAsync(uint id);
  }
}