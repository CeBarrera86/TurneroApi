using Microsoft.AspNetCore.Authorization; // Add if you plan to use authorization
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Historial;
using TurneroApi.Interfaces;
using TurneroApi.Models; // If you still need direct model references for some reason

namespace TurneroApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HistorialController : ControllerBase
  {
    private readonly IHistorialService _historialService;
    public HistorialController(IHistorialService historialService)
    {
      _historialService = historialService;
    }

    // GET: api/Historial/ticket/{ticketId}
    // Para obtener todas las entradas de historial para un ticket específico
    [HttpGet("ticket/{ticketId}")]
    [Authorize(Policy = "ver_historial")]
    public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistorialByTicket(ulong ticketId)
    {
      var historiales = await _historialService.GetHistorialByTicketIdAsync(ticketId);
      if (!historiales.Any())
      {
        return NotFound($"No se encontró historial para el TicketId '{ticketId}'.");
      }
      return Ok(historiales);
    }

    // GET: api/Historial/turno/{turnoId}
    // Opcional: Para obtener todas las entradas de historial para un turno específico
    [HttpGet("turno/{turnoId}")]
    [Authorize(Policy = "ver_historial")]
    public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistorialByTurno(ulong turnoId)
    {
      var historiales = await _historialService.GetHistorialByTurnoIdAsync(turnoId);
      if (!historiales.Any())
      {
        return NotFound($"No se encontró historial para el TurnoId '{turnoId}'.");
      }
      return Ok(historiales);
    }

    // No hay GET por ID directo, ni PUT, POST, DELETE si se maneja como un ledger inmutable.
    // La creación de entradas de historial se realizaría internamente por otros servicios.
    /*
    // Ejemplo de cómo OTRO SERVICIO (ej. TicketService o TurnoService) llamaría a HistorialService
    // Este no sería un endpoint público en HistorialController
    private async Task LogTicketStateChange(
      ulong ticketId,
      int newEstadoId, // ← int en lugar de uint
      int? puestoId = null, // ← int? en lugar de uint?
      ulong? turnoId = null,
      int? usuarioId = null, // ← int? en lugar de uint?
      string? comentarios = null)
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