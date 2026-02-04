using TurneroApi.DTOs.Historial;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces
{
  public interface IHistorialService
  {
    Task<PagedResult<HistorialDto>> GetHistorialByTicketIdAsync(ulong ticketId, int page, int pageSize);
    Task<PagedResult<HistorialDto>> GetHistorialByTurnoIdAsync(ulong turnoId, int page, int pageSize);
    Task<(Historial? historial, string? errorMessage)> AddHistorialEntryAsync(HistorialCrearDto historialCrearDto);
  }
}