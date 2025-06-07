using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public TurnoController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Turno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurnoDto>>> GetTurnos()
        {
            var turnos = await _context.Turnos.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TurnoDto>>(turnos));
        }

        // GET: api/Turno/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TurnoDto>> GetTurno(ulong id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            return Ok(_mapper.Map<TurnoDto>(turno));
        }

        // PUT: api/Turno/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurno(ulong id, TurnoDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el del DTO.");

            var entity = await _context.Turnos.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Turno
        [HttpPost]
        public async Task<ActionResult<TurnoDto>> PostTurno(TurnoDto dto)
        {
            var entity = _mapper.Map<Turno>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            _context.Turnos.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTurno), new { id = entity.Id }, _mapper.Map<TurnoDto>(entity));
        }

        // DELETE: api/Turno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurno(ulong id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TurnoExists(ulong id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}
