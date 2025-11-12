using TurneroApi.DTOs.Ticket;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface ITicketService
  {
    Task<IEnumerable<Ticket>> GetTicketsAsync();
    Task<IEnumerable<Ticket>> GetTicketsFiltrados(DateTime fecha, int sectorIdOrigen, int estadoId);
    Task<Ticket?> GetTicketAsync(ulong id);
    Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto);
    Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto dto, int? usuarioId);
    Task<Ticket?> LlamarTicketAsync(ulong id, int? usuarioId);
    Task<Ticket?> BuscarTicket(string letra, int numero);
  }
}