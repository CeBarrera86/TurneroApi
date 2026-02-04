using TurneroApi.DTOs.Estado;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces
{
  public interface IEstadoService
  {
    Task<PagedResult<EstadoDto>> GetEstadosAsync(int page, int pageSize);
    Task<EstadoDto?> GetEstadoAsync(int id);
    Task<(Estado? estado, string? errorMessage)> CreateEstadoAsync(Estado estado);
    Task<(Estado? estado, string? errorMessage)> UpdateEstadoAsync(int id, EstadoActualizarDto estadoActualizarDto);
    Task<(bool deleted, string? errorMessage)> DeleteEstadoAsync(int id);
  }
}
