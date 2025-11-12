using TurneroApi.DTOs.Usuario;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IUsuarioService
{
  Task<IEnumerable<Usuario>> GetUsuariosAsync();
  Task<Usuario?> GetUsuarioAsync(int id);
  Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario);
  Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(int id, UsuarioActualizarDto usuarioActualizarDto);
  Task<bool> DeleteUsuarioAsync(int id);
}