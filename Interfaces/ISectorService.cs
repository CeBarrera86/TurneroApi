using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface ISectorService
    {
        Task<IEnumerable<Sector>> GetSectoresAsync();
        Task<Sector?> GetSectorAsync(uint id);
        Task<(Sector? sector, string? errorMessage)> CreateSectorAsync(Sector sector);
        Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(uint id, SectorActualizarDto sectorActualizarDto);
        Task<bool> DeleteSectorAsync(uint id);
    }
}