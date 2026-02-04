using TurneroApi.DTOs.Permiso;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IPermisoService
{
  Task<PagedResult<PermisoDto>> GetPermisosAsync(int page, int pageSize);
  Task<PermisoDto?> GetPermisoAsync(int id);
  Task<(Permiso? permiso, string? errorMessage)> CreatePermisoAsync(Permiso permiso);
  Task<(Permiso? permiso, string? errorMessage)> UpdatePermisoAsync(int id, PermisoActualizarDto dto);
  Task<(bool deleted, string? errorMessage)> DeletePermisoAsync(int id);
}