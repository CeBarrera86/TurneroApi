using TurneroApi.DTOs;
using TurneroApi.Models;
namespace TurneroApi.Interfaces
{
    public interface IEstadoService
    {
        Task<IEnumerable<Estado>> GetEstadosAsync();
        Task<Estado?> GetEstadoAsync(uint id);
        Task<(Estado? estado, string? errorMessage)> CreateEstadoAsync(Estado estado);
        Task<(Estado? estado, string? errorMessage)> UpdateEstadoAsync(uint id, EstadoActualizarDto estadoActualizarDto);
        Task<bool> DeleteEstadoAsync(uint id);
    }
}