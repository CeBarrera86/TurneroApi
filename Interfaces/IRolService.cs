using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> GetRolesAsync();
        Task<Rol?> GetRolAsync(uint id);
        Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol);
        Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(uint id, RolActualizarDto rolActualizarDto);
        Task<bool> DeleteRolAsync(uint id);
    }
}