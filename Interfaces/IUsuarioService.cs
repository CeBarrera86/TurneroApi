using TurneroApi.DTOs.Usuario;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IUsuarioService
{
  Task<PagedResult<UsuarioDto>> GetUsuariosAsync(int page, int pageSize);
  Task<UsuarioDto?> GetUsuarioAsync(int id);
  Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario);
  Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(int id, UsuarioActualizarDto usuarioActualizarDto);
  Task<bool> DeleteUsuarioAsync(int id);
}