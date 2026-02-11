using TurneroApi.DTOs.Ticket;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface ITicketService
  {
    Task<List<TicketDto>> GetTicketsAsync();
    Task<List<TicketDto>> GetTicketsFiltrados(DateTime fecha, int sectorIdOrigen, int estadoId);
    Task<TicketDto?> GetTicketAsync(ulong id);
    Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto);
    Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto dto, int? usuarioId, int? puestoId);
    Task<TicketDto?> LlamarTicketAsync(ulong id, int? usuarioId, int? puestoId);
    Task<TicketDto?> BuscarTicket(string letra, int numero);
  }
}