using AutoMapper;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Ticket;
using TurneroApi.Interfaces;
using TurneroApi.Enums;

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
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
      var tickets = await _ticketService.GetTicketsAsync();
      var ticketsDto = _mapper.Map<IEnumerable<TicketDto>>(tickets);
      return Ok(ticketsDto);
    }

    [HttpGet("filtrados")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsFiltrados(
      [FromQuery] DateTime fecha,
      [FromQuery] int sectorIdOrigen,
      [FromQuery] int estadoId = 4)
    {
      var tickets = await _ticketService.GetTicketsFiltrados(fecha, sectorIdOrigen, estadoId);
      var ticketsDto = _mapper.Map<IEnumerable<TicketDto>>(tickets);
      return Ok(ticketsDto);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetTicket(ulong id)
    {
      var ticket = await _ticketService.GetTicketAsync(id);
      if (ticket == null)
      {
        return NotFound();
      }
      var ticketDto = _mapper.Map<TicketDto>(ticket);
      return Ok(ticketDto);
    }

    // GET: api/Ticket/search?letra=C&numero=0&date=2025-06-12
    [HttpGet("search")]
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

      var ticketDto = _mapper.Map<TicketDto>(ticketBuscado);
      return Ok(ticketDto);
    }

    // POST: api/Ticket (Creación de un ticket desde una PC pública)
    [HttpPost]
    // No [Authorize] aquí, como lo solicitaste.
    public async Task<ActionResult<TicketDto>> PostTicket([FromBody] TicketCrearDto ticketCrearDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var (createdTicket, errorMessage) = await _ticketService.CrearTicket(ticketCrearDto);

      if (createdTicket == null)
      {
        return BadRequest(new { message = errorMessage });
      }

      var ticketDto = _mapper.Map<TicketDto>(createdTicket);
      return CreatedAtAction(nameof(GetTicket), new { id = ticketDto.Id }, ticketDto);
    }

    [HttpPost("{id}/llamar")]
    [Authorize(Roles = "Admin, Usuario")]
    public async Task<IActionResult> LlamarTicket(ulong id)
    {
      var usuarioId = ObtenerUsuarioActualId();
      var ticket = await _ticketService.LlamarTicketAsync(id, usuarioId);
      if (ticket == null) return NotFound(new { message = "Ticket no encontrado." });

      var dto = _mapper.Map<TicketDto>(ticket);
      return Ok(dto);
    }

    // PATCH: api/Ticket/5 (Actualización parcial de Estado y SectorIdActual)
    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin, Usuario")]
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