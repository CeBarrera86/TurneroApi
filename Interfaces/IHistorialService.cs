using TurneroApi.DTOs.Historial;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface IHistorialService
  {
    Task<List<HistorialDto>> GetHistorialByTicketIdAsync(ulong ticketId);
    Task<List<HistorialDto>> GetHistorialByTurnoIdAsync(ulong turnoId);
    Task<(Historial? historial, string? errorMessage)> AddHistorialEntryAsync(HistorialCrearDto historialCrearDto);
  }
}