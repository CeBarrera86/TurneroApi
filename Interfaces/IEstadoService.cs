using TurneroApi.DTOs.Estado;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface IEstadoService
  {
    Task<List<EstadoDto>> GetEstadosAsync();
    Task<EstadoDto?> GetEstadoAsync(int id);
    Task<(Estado? estado, string? errorMessage)> CreateEstadoAsync(Estado estado);
    Task<(Estado? estado, string? errorMessage)> UpdateEstadoAsync(int id, EstadoActualizarDto estadoActualizarDto);
    Task<(bool deleted, string? errorMessage)> DeleteEstadoAsync(int id);
  }
}
