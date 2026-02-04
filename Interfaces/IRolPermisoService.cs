using TurneroApi.DTOs.RolPermiso;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IRolPermisoService
{
  Task<PagedResult<RolPermisoDto>> GetRolPermisosAsync(int page, int pageSize);
  Task<RolPermisoDto?> GetRolPermisoAsync(int rolId, int permisoId);
  Task<(RolPermiso? rolPermiso, string? errorMessage)> CreateRolPermisoAsync(RolPermisoCrearDto dto);
  Task<(bool deleted, string? errorMessage)> DeleteRolPermisoAsync(int rolId, int permisoId);
}