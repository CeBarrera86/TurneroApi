using TurneroApi.DTOs.RolPermiso;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IRolPermisoService
{
  Task<IEnumerable<RolPermiso>> GetRolPermisosAsync();
  Task<RolPermiso?> GetRolPermisoAsync(int rolId, int permisoId);
  Task<(RolPermiso? rolPermiso, string? errorMessage)> CreateRolPermisoAsync(RolPermisoCrearDto dto);
  Task<(bool deleted, string? errorMessage)> DeleteRolPermisoAsync(int rolId, int permisoId);
}