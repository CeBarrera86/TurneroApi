using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models;
using TurneroApi.DTOs;
using AutoMapper;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public TicketController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.ClienteNavigation)
                .Include(t => t.EstadoNavigation)
                .Include(t => t.SectorNavigation)
                .ToListAsync();

            var ticketDtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);
            return Ok(ticketDtos);
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(ulong id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.ClienteNavigation)
                .Include(t => t.EstadoNavigation)
                .Include(t => t.SectorNavigation)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return Ok(ticketDto);
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<ActionResult<TicketDto>> PostTicket(TicketDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<TicketDto>(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, createdDto);
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(ulong id, TicketDto ticketDto)
        {
            if (id != ticketDto.Id)
            {
                return BadRequest();
            }

            var ticket = _mapper.Map<Ticket>(ticketDto);
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(ulong id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(ulong id)
        {
            return _context.Tickets.Any(t => t.Id == id);
        }
    }
}
