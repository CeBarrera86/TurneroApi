using AutoMapper;
using Microsoft.AspNetCore.Authorization; // Add if you plan to use authorization
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models; // If you still need direct model references for some reason

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialController : ControllerBase
    {
        private readonly IHistorialService _historialService;
        private readonly IMapper _mapper;

        public HistorialController(IHistorialService historialService, IMapper mapper)
        {
            _historialService = historialService;
            _mapper = mapper;
        }

        // GET: api/Historial/ticket/{ticketId}?page=1&pageSize=10
        // Para obtener todas las entradas de historial para un ticket específico
        [HttpGet("ticket/{ticketId}")]
        // [Authorize(Roles = "Admin, Usuario")] // Considerar si todos pueden ver el historial
        public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistorialByTicket(ulong ticketId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var historiales = await _historialService.GetHistorialByTicketIdAsync(ticketId, page, pageSize);
            if (!historiales.Any())
            {
                return NotFound($"No se encontró historial para el TicketId '{ticketId}'.");
            }
            var historialesDto = _mapper.Map<IEnumerable<HistorialDto>>(historiales);
            return Ok(historialesDto);
        }

        // GET: api/Historial/turno/{turnoId}?page=1&pageSize=10
        // Opcional: Para obtener todas las entradas de historial para un turno específico
        [HttpGet("turno/{turnoId}")]
        // [Authorize(Roles = "Admin, Usuario")]
        public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistorialByTurno(ulong turnoId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var historiales = await _historialService.GetHistorialByTurnoIdAsync(turnoId, page, pageSize);
            if (!historiales.Any())
            {
                return NotFound($"No se encontró historial para el TurnoId '{turnoId}'.");
            }
            var historialesDto = _mapper.Map<IEnumerable<HistorialDto>>(historiales);
            return Ok(historialesDto);
        }

        // No hay GET por ID directo, ni PUT, POST, DELETE si se maneja como un ledger inmutable.
        // La creación de entradas de historial se realizaría internamente por otros servicios.
        /*
        // Ejemplo de cómo OTRO SERVICIO (ej. TicketService o TurnoService) llamaría a HistorialService
        // Este no sería un endpoint público en HistorialController
        private async Task LogTicketStateChange(ulong ticketId, uint newEstadoId, uint? puestoId = null, ulong? turnoId = null, uint? usuarioId = null, string? comentarios = null)
        {
            var historialCrearDto = new HistorialCrearDto
            {
                TicketId = ticketId,
                EstadoId = newEstadoId,
                PuestoId = puestoId,
                TurnoId = turnoId,
                UsuarioId = usuarioId,
                Comentarios = comentarios
            };
            await _historialService.AddHistorialEntryAsync(historialCrearDto);
        }
        */
    }
}