using TurneroApi.DTOs.MostradorSector;

namespace TurneroApi.Interfaces;

public interface IMostradorSectorService
{
  Task<(bool ok, string? error)> AsociarAsync(int mostradorId, int sectorId);
  Task<(bool ok, string? error)> DesasociarAsync(int mostradorId, int sectorId);

  Task<IEnumerable<MostradorSectorItemDto>> GetSectoresPorMostradorAsync(int mostradorId);
  Task<IEnumerable<MostradorSectorItemDto>> GetMostradoresPorSectorAsync(int sectorId);
}