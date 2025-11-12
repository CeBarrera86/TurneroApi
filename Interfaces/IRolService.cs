using TurneroApi.DTOs.Rol;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IRolService
{
  Task<IEnumerable<Rol>> GetRolesAsync();
  Task<Rol?> GetRolAsync(int id); // ← int
  Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol);
  Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(int id, RolActualizarDto rolActualizarDto); // ← int
  Task<(bool deleted, string? errorMessage)> DeleteRolAsync(int id); // ← int
}