using TurneroApi.DTOs.Usuario;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IUsuarioService
{
  Task<List<UsuarioDto>> GetUsuariosAsync();
  Task<UsuarioDto?> GetUsuarioAsync(int id);
  Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario);
  Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(int id, UsuarioActualizarDto usuarioActualizarDto);
  Task<bool> DeleteUsuarioAsync(int id);
}