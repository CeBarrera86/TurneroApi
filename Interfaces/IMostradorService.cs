using TurneroApi.DTOs.Mostrador;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IMostradorService
{
  Task<IEnumerable<Mostrador>> GetMostradoresAsync();
  Task<Mostrador?> GetMostradorAsync(int id);
  Task<(Mostrador? mostrador, string? errorMessage)> CreateMostradorAsync(Mostrador mostrador);
  Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(int id, MostradorActualizarDto dto);
  Task<bool> DeleteMostradorAsync(int id);
}