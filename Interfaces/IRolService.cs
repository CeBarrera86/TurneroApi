using TurneroApi.DTOs.Rol;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IRolService
{
  Task<PagedResult<RolDto>> GetRolesAsync(int page, int pageSize);
  Task<RolDto?> GetRolAsync(int id); // ← int
  Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol);
  Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(int id, RolActualizarDto rolActualizarDto); // ← int
  Task<(bool deleted, string? errorMessage)> DeleteRolAsync(int id); // ← int
}