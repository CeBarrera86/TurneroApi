using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class HistorialService : IHistorialService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public HistorialService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Historial>> GetHistorialByTicketIdAsync(ulong ticketId, int page, int pageSize)
        {
            return await _context.Historiales
                .Include(h => h.EstadoNavigation)
                .Include(h => h.PuestoNavigation)
                .Include(h => h.TurnoNavigation)
                .Include(h => h.UsuarioNavigation)
                .Include(h => h.TicketNavigation) // Si necesitas detalles del ticket en cada historial
                .Where(h => h.TicketId == ticketId)
                .OrderBy(h => h.Fecha) // Ordenar por fecha para una línea de tiempo
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Historial>> GetHistorialByTurnoIdAsync(ulong turnoId, int page, int pageSize)
        {
            return await _context.Historiales
                .Include(h => h.EstadoNavigation)
                .Include(h => h.PuestoNavigation)
                .Include(h => h.TurnoNavigation)
                .Include(h => h.UsuarioNavigation)
                .Include(h => h.TicketNavigation) // Si necesitas detalles del ticket
                .Where(h => h.TurnoId == turnoId)
                .OrderBy(h => h.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(Historial? historial, string? errorMessage)> AddHistorialEntryAsync(HistorialCrearDto historialCrearDto)
        {
            // Validaciones para asegurar la existencia de los IDs relacionados
            var ticketExists = await _context.Tickets.AnyAsync(t => t.Id == historialCrearDto.TicketId);
            if (!ticketExists)
            {
                return (null, $"TicketId '{historialCrearDto.TicketId}' no existe.");
            }

            var estadoExists = await _context.Estados.AnyAsync(e => e.Id == historialCrearDto.EstadoId);
            if (!estadoExists)
            {
                return (null, $"EstadoId '{historialCrearDto.EstadoId}' no existe.");
            }

            if (historialCrearDto.PuestoId.HasValue)
            {
                var puestoExists = await _context.Puestos.AnyAsync(p => p.Id == historialCrearDto.PuestoId.Value);
                if (!puestoExists)
                {
                    return (null, $"PuestoId '{historialCrearDto.PuestoId.Value}' no existe.");
                }
            }

            if (historialCrearDto.TurnoId.HasValue)
            {
                var turnoExists = await _context.Turnos.AnyAsync(t => t.Id == historialCrearDto.TurnoId.Value);
                if (!turnoExists)
                {
                    return (null, $"TurnoId '{historialCrearDto.TurnoId.Value}' no existe.");
                }
            }

            if (historialCrearDto.UsuarioId.HasValue)
            {
                var userExists = await _context.Usuarios.AnyAsync(u => u.Id == historialCrearDto.UsuarioId.Value);
                if (!userExists)
                {
                    return (null, $"UsuarioId '{historialCrearDto.UsuarioId.Value}' no existe.");
                }
            }

            var historial = _mapper.Map<Historial>(historialCrearDto);
            historial.Fecha = DateTime.Now; // La fecha se establece automáticamente al momento de la creación.

            _context.Historiales.Add(historial);

            try
            {
                await _context.SaveChangesAsync();

                // Cargar navegaciones para el DTO de respuesta, si es necesario
                await _context.Entry(historial).Reference(h => h.TicketNavigation).LoadAsync();
                await _context.Entry(historial).Reference(h => h.EstadoNavigation).LoadAsync();
                if (historial.PuestoId.HasValue) await _context.Entry(historial).Reference(h => h.PuestoNavigation).LoadAsync();
                if (historial.TurnoId.HasValue) await _context.Entry(historial).Reference(h => h.TurnoNavigation).LoadAsync();
                if (historial.UsuarioId.HasValue) await _context.Entry(historial).Reference(h => h.UsuarioNavigation).LoadAsync();

                return (historial, null);
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al añadir entrada al historial: {ex.Message}");
            }
        }
    }
}