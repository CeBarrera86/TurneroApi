using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<Ticket?> GetTicketAsync(ulong id);
        Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto);
        Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto ticketActualizarDto); 
        Task<Ticket?> BuscarTicket(string letra, uint numero);
    }
}