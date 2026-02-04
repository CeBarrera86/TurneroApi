using TurneroApi.DTOs.Ticket;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces
{
  public interface ITicketService
  {
    Task<PagedResult<TicketDto>> GetTicketsAsync(int page, int pageSize);
    Task<PagedResult<TicketDto>> GetTicketsFiltrados(DateTime fecha, int sectorIdOrigen, int estadoId, int page, int pageSize);
    Task<TicketDto?> GetTicketAsync(ulong id);
    Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto);
    Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto dto, int? usuarioId);
    Task<TicketDto?> LlamarTicketAsync(ulong id, int? usuarioId);
    Task<TicketDto?> BuscarTicket(string letra, int numero);
  }
}