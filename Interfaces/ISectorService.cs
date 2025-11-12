using TurneroApi.DTOs.Sector;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface ISectorService
{
  Task<IEnumerable<Sector>> GetSectoresAsync();
  Task<Sector?> GetSectorAsync(int id);
  Task<(Sector? sector, string? errorMessage)> CreateSectorAsync(Sector sector);
  Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(int id, SectorActualizarDto sectorActualizarDto);
  Task<IEnumerable<Sector>> GetSectoresActivosAsync();
  Task<IEnumerable<Sector>> GetSectoresActivosPadresAsync();
  Task<(bool deleted, string? errorMessage)> DeleteSectorAsync(int id);
}
