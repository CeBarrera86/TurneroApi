using AutoMapper;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Ticket;
using TurneroApi.Interfaces;
using TurneroApi.Enums;
using TurneroApi.Utils;

namespace TurneroApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TicketController : ControllerBase
  {
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;

    public TicketController(ITicketService ticketService, IMapper mapper)
    {
      _ticketService = ticketService;
      _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "ver_ticket")]
    public async Task<ActionResult<PagedResponse<TicketDto>>> GetTickets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (!PaginationHelper.IsValid(page, pageSize, out var message))
      {
        return BadRequest(new { message });
      }

      var result = await _ticketService.GetTicketsAsync(page, pageSize);
      return Ok(new PagedResponse<TicketDto>(result.Items, page, pageSize, result.Total));
    }

    [HttpGet("filtrados")]
    [Authorize(Policy = "ver_ticket")]
    public async Task<ActionResult<PagedResponse<TicketDto>>> GetTicketsFiltrados(
      [FromQuery] DateTime fecha,
      [FromQuery] int sectorIdOrigen,
      [FromQuery] int estadoId = 4,
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10)
    {
      if (!PaginationHelper.IsValid(page, pageSize, out var message))
      {
        return BadRequest(new { message });
      }

      var result = await _ticketService.GetTicketsFiltrados(fecha, sectorIdOrigen, estadoId, page, pageSize);
      return Ok(new PagedResponse<TicketDto>(result.Items, page, pageSize, result.Total));
    }


    [HttpGet("{id}")]
    [Authorize(Policy = "ver_ticket")]
    public async Task<ActionResult<TicketDto>> GetTicket(ulong id)
    {
      var ticket = await _ticketService.GetTicketAsync(id);
      if (ticket == null)
      {
        return NotFound();
      }
      return Ok(ticket);
    }

    // GET: api/Ticket/search?letra=C&numero=0&date=2025-06-12
    [HttpGet("search")]
    [Authorize(Policy = "ver_ticket")]
    public async Task<ActionResult<TicketDto>> GetTicketByLetraAndNumeroAndDate([FromQuery] string ticket)
    {
      if (string.IsNullOrWhiteSpace(ticket))
      {
        return BadRequest("El parámetro 'ticket' es obligatorio y no puede estar vacío.");
      }

      string ticketTrim = ticket.Trim();

      // Separar la parte numérica del texto
      Match match = Regex.Match(ticketTrim, @"^([A-Za-z]{1,2})\s*(\d+)$");
      if (!match.Success)
      {
        return BadRequest("Formato de ticket inválido. Ejemplos válidos: 'C2', 'R 3', 'Ap5'.");
      }

      string letra = match.Groups[1].Value.ToUpper();
      int numero; // ahora int
      if (!int.TryParse(match.Groups[2].Value, out numero))
      {
        return BadRequest("Número de ticket inválido.");
      }

      var ticketBuscado = await _ticketService.BuscarTicket(letra, numero);

      if (ticketBuscado == null)
      {
        return NotFound($"No se encontró ningún ticket con letra '{letra}', número '{numero}'.");
      }

      return Ok(ticketBuscado);
    }

    // POST: api/Ticket (Endpoint exclusivo del Totem para crear tickets)
    [HttpPost("totem")]
    [Authorize(Policy = "TotemAccess")]
    public async Task<ActionResult<TicketDto>> PostTicketTotem([FromBody] TicketCrearDto ticketCrearDto)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var (createdTicket, errorMessage) = await _ticketService.CrearTicket(ticketCrearDto);
      if (createdTicket == null) return BadRequest(new { message = errorMessage });

      var ticketDto = _mapper.Map<TicketDto>(createdTicket);
      return CreatedAtAction(nameof(GetTicket), new { id = ticketDto.Id }, ticketDto);
    }

    [HttpPost("{id}/llamar")]
    [Authorize(Policy = "editar_ticket")]
    public async Task<IActionResult> LlamarTicket(ulong id)
    {
      var usuarioId = ObtenerUsuarioActualId();
      var ticket = await _ticketService.LlamarTicketAsync(id, usuarioId);
      if (ticket == null) return NotFound(new { message = "Ticket no encontrado." });

      return Ok(ticket);
    }

    // PATCH: api/Ticket/5 (Actualización parcial de Estado y SectorIdActual)
    [HttpPatch("{id}")]
    [Authorize(Policy = "editar_ticket")]
    public async Task<IActionResult> PatchTicket(ulong id, [FromBody] TicketActualizarDto ticketActualizarDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var usuarioId = ObtenerUsuarioActualId(); // Extraer desde Claims

      var (updatedTicket, errorMessage) = await _ticketService.ActualizarTicket(id, ticketActualizarDto, usuarioId);

      if (updatedTicket == null)
      {
        if (errorMessage == "Ticket no encontrado.")
          return NotFound(new { message = errorMessage });

        return BadRequest(new { message = errorMessage });
      }

      var ticketDto = _mapper.Map<TicketDto>(updatedTicket);
      return Ok(ticketDto);
    }

    private int? ObtenerUsuarioActualId()
    {
      var claim = User?.FindFirst("id");
      return claim != null ? int.Parse(claim.Value) : null;
    }

  }
}