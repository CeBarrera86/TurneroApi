using TurneroApi.DTOs.RolPermiso;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IRolPermisoService
{
  Task<List<RolPermisoDto>> GetRolPermisosAsync();
  Task<RolPermisoDto?> GetRolPermisoAsync(int rolId, int permisoId);
  Task<(RolPermiso? rolPermiso, string? errorMessage)> CreateRolPermisoAsync(RolPermisoCrearDto dto);
  Task<(bool deleted, string? errorMessage)> DeleteRolPermisoAsync(int rolId, int permisoId);
}