using TurneroApi.DTOs.Mostrador;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IMostradorService
{
  Task<PagedResult<MostradorDto>> GetMostradoresAsync(int page, int pageSize);
  Task<MostradorDto?> GetMostradorAsync(int id);
  Task<(Mostrador? mostrador, string? errorMessage)> CreateMostradorAsync(Mostrador mostrador);
  Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(int id, MostradorActualizarDto dto);
  Task<bool> DeleteMostradorAsync(int id);
}