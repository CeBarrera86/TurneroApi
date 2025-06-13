using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public TicketService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.ClienteNavigation)
                .Include(t => t.EstadoNavigation)
                .Include(t => t.SectorIdActualNavigation)
                .Include(t => t.SectorIdOrigenNavigation)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketAsync(ulong id)
        {
            return await _context.Tickets
                .Include(t => t.ClienteNavigation)
                .Include(t => t.EstadoNavigation)
                .Include(t => t.SectorIdActualNavigation)
                .Include(t => t.SectorIdOrigenNavigation)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto)
        {
            string letra = ticketCrearDto.Letra.ToUpper();
            var today = DateTime.Today;
            uint numero = 0; // Empieza en 0 cada día

            // Validaciones
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == ticketCrearDto.ClienteId);
            if (!clienteExiste)
            {
                return (null, $"El ClienteId '{ticketCrearDto.ClienteId}' proporcionado no es válido.");
            }

            var ultimoTicket = await _context.Tickets.Where(t => t.Letra == letra && t.Fecha.Date == today)
                .OrderByDescending(t => t.Numero).FirstOrDefaultAsync();
            if (ultimoTicket != null)
            {
                numero = ultimoTicket.Numero + 1;
            }

            var ticketExistente = await _context.Tickets.AnyAsync(t => t.Letra == letra && t.Numero == numero && t.Fecha.Date == today);
            if (ticketExistente)
            {
                return (null, $"Ya existe un ticket con la combinación Letra '{letra}' y Número '{numero}' para la fecha actual. Intente nuevamente.");
            }

            var sectorOrigenExiste = await _context.Sectores.AnyAsync(s => s.Id == ticketCrearDto.SectorIdOrigen);
            if (!sectorOrigenExiste)
            {
                return (null, $"El SectorIdOrigen '{ticketCrearDto.SectorIdOrigen}' proporcionado no es válido.");
            }

            var estadoDisponible = await _context.Estados.FirstOrDefaultAsync(e => e.Id == 4 && e.Descripcion == "DISPONIBLE");
            if (estadoDisponible == null)
            {
                return (null, "El estado 'DISPONIBLE' (ID 4) no está configurado en la base de datos.");
            }

            var ticket = _mapper.Map<Ticket>(ticketCrearDto);

            ticket.Letra = letra;
            ticket.Numero = numero;
            ticket.ClienteId = ticketCrearDto.ClienteId;
            ticket.Fecha = DateTime.Now;
            ticket.SectorIdOrigen = ticketCrearDto.SectorIdOrigen;
            ticket.EstadoId = 4; // (Estado.descripcion = "DISPONIBLE")

            _context.Tickets.Add(ticket);

            try
            {
                await _context.SaveChangesAsync();
                // Cargar navegaciones para el DTO de respuesta
                await _context.Entry(ticket).Reference(t => t.ClienteNavigation).LoadAsync();
                await _context.Entry(ticket).Reference(t => t.EstadoNavigation).LoadAsync();
                // Manejar SectorIdActualNavigation, ya que ahora es nullable
                if (ticket.SectorIdActual.HasValue)
                {
                    await _context.Entry(ticket).Reference(t => t.SectorIdActualNavigation!).LoadAsync();
                }
                await _context.Entry(ticket).Reference(t => t.SectorIdOrigenNavigation).LoadAsync();
                return (ticket, null);
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al crear el ticket: {ex.Message}");
            }
        }

        public async Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto ticketActualizarDto)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return (null, "Ticket no encontrado.");
            }

            bool changed = false;

            if (ticketActualizarDto.SectorIdActual.HasValue)
            {
                var nuevoSectorId = ticketActualizarDto.SectorIdActual.Value;

                if (nuevoSectorId == 0)
                {
                    if (ticket.SectorIdActual != null)
                    {
                        ticket.SectorIdActual = null;
                        changed = true;
                    }
                }
                else
                {
                    if (nuevoSectorId != ticket.SectorIdActual)
                    {
                        // Validar que el sector exista
                        var sectorExiste = await _context.Sectores.AnyAsync(s => s.Id == nuevoSectorId);
                        if (!sectorExiste)
                        {
                            return (null, $"El SectorIdActual '{nuevoSectorId}' proporcionado no es válido.");
                        }

                        ticket.SectorIdActual = nuevoSectorId;
                        changed = true;
                    }
                }
            }

            // Para actualizar EstadoId si el DTO lo provee y es diferente al actual
            if (ticketActualizarDto.EstadoId.HasValue && ticketActualizarDto.EstadoId.Value != ticket.EstadoId)
            {
                var nuevoEstadoId = ticketActualizarDto.EstadoId.Value;

                // Validar que el EstadoId proporcionado exista en la base de datos (SIN RESTRICCIÓN DE CUALES)
                var estadoExiste = await _context.Estados.AnyAsync(e => e.Id == nuevoEstadoId);
                if (!estadoExiste)
                {
                    return (null, $"El EstadoId '{nuevoEstadoId}' proporcionado no es válido.");
                }

                ticket.EstadoId = nuevoEstadoId;
                changed = true;

                // Asumiendo: 2-CULMINADO, 4-DISPONIBLE, 5-ELIMINADO
                if (nuevoEstadoId == 2 || nuevoEstadoId == 4 || nuevoEstadoId == 5)
                {
                    ticket.SectorIdActual = null;
                }
            }

            // Actualizar 'Actualizado' solo si hubo algún cambio
            if (changed)
            {
                ticket.Actualizado = DateTime.Now;
            }
            else
            {
                return (ticket, null);
            }

            try
            {
                await _context.SaveChangesAsync();
                // Cargar navegaciones para el DTO de respuesta
                await _context.Entry(ticket).Reference(t => t.ClienteNavigation).LoadAsync();
                await _context.Entry(ticket).Reference(t => t.EstadoNavigation).LoadAsync();
                // Carga condicional para SectorIdActualNavigation
                if (ticket.SectorIdActual.HasValue)
                {
                    await _context.Entry(ticket).Reference(t => t.SectorIdActualNavigation!).LoadAsync();
                }
                await _context.Entry(ticket).Reference(t => t.SectorIdOrigenNavigation).LoadAsync();
                return (ticket, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tickets.AnyAsync(e => e.Id == id))
                {
                    return (null, "Ticket no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al actualizar el ticket: {ex.Message}");
            }
        }

        public async Task<Ticket?> BuscarTicket(string letra, uint numero)
        {
            var today = DateTime.Today;
            var ticket = await _context.Tickets
                .Include(t => t.ClienteNavigation)
                .Include(t => t.EstadoNavigation)
                .Include(t => t.SectorIdActualNavigation)
                .Include(t => t.SectorIdOrigenNavigation)
                .FirstOrDefaultAsync(t => t.Letra == letra && t.Numero == numero && t.Fecha.Date == today);
            return ticket;
        }
    }
}