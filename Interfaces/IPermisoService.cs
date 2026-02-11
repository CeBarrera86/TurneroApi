using TurneroApi.DTOs.Permiso;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IPermisoService
{
  Task<List<PermisoDto>> GetPermisosAsync();
  Task<PermisoDto?> GetPermisoAsync(int id);
  Task<(Permiso? permiso, string? errorMessage)> CreatePermisoAsync(Permiso permiso);
  Task<(Permiso? permiso, string? errorMessage)> UpdatePermisoAsync(int id, PermisoActualizarDto dto);
  Task<(bool deleted, string? errorMessage)> DeletePermisoAsync(int id);
}