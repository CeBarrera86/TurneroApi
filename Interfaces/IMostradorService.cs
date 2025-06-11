using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IMostradorService
    {
        Task<(Mostrador? mostrador, string? errorMessage)> CreateMostradorAsync(Mostrador mostrador);
        Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(uint id, MostradorActualizarDto mostradorActualizarDto);
        Task<bool> DeleteMostradorAsync(uint id);
        Task<IEnumerable<Mostrador>> GetMostradoresAsync();
        Task<Mostrador?> GetMostradorAsync(uint id);
    }
}