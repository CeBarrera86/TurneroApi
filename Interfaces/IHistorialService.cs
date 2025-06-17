using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IHistorialService
    {
        Task<IEnumerable<Historial>> GetHistorialByTicketIdAsync(ulong ticketId, int page, int pageSize);
        Task<IEnumerable<Historial>> GetHistorialByTurnoIdAsync(ulong turnoId, int page, int pageSize);
        Task<(Historial? historial, string? errorMessage)> AddHistorialEntryAsync(HistorialCrearDto historialCrearDto);
    }
}