using TurneroApi.DTOs.Sector;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface ISectorService
{
  Task<List<SectorDto>> GetSectoresAsync();
  Task<List<SectorDto>> GetSectoresTotemAsync();
  Task<List<SectorDto>> GetTramitesTotemAsync();
  Task<SectorDto?> GetSectorAsync(int id);
  Task<(Sector? sector, string? errorMessage)> CreateSectorAsync(Sector sector);
  Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(int id, SectorActualizarDto sectorActualizarDto);
  Task<List<SectorDto>> GetSectoresActivosAsync();
  Task<List<SectorDto>> GetSectoresActivosPadresAsync();
  Task<(bool deleted, string? errorMessage)> DeleteSectorAsync(int id);
}
