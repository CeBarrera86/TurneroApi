using TurneroApi.DTOs.Sector;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface ISectorService
{
  Task<PagedResult<SectorDto>> GetSectoresAsync(int page, int pageSize);
  Task<PagedResult<SectorDto>> GetSectoresTotemAsync(int page, int pageSize);
  Task<PagedResult<SectorDto>> GetTramitesTotemAsync(int page, int pageSize);
  Task<SectorDto?> GetSectorAsync(int id);
  Task<(Sector? sector, string? errorMessage)> CreateSectorAsync(Sector sector);
  Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(int id, SectorActualizarDto sectorActualizarDto);
  Task<PagedResult<SectorDto>> GetSectoresActivosAsync(int page, int pageSize);
  Task<PagedResult<SectorDto>> GetSectoresActivosPadresAsync(int page, int pageSize);
  Task<(bool deleted, string? errorMessage)> DeleteSectorAsync(int id);
}
