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
    public class HistorialController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public HistorialController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Historial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistoriales()
        {
            var historiales = await _context.Historiales.ToListAsync();
            var historialesDto = _mapper.Map<IEnumerable<HistorialDto>>(historiales);
            return Ok(historialesDto);
        }

        // GET: api/Historial/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialDto>> GetHistorial(ulong id)
        {
            var historial = await _context.Historiales.FindAsync(id);

            if (historial == null)
                return NotFound();

            return Ok(_mapper.Map<HistorialDto>(historial));
        }

        // PUT: api/Historial/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorial(ulong id, HistorialDto historialDto)
        {
            if (id != historialDto.Id)
                return BadRequest("El ID de la URL no coincide con el DTO.");

            var historial = await _context.Historiales.FindAsync(id);
            if (historial == null)
                return NotFound();

            _mapper.Map(historialDto, historial);
            historial.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Historial
        [HttpPost]
        public async Task<ActionResult<HistorialDto>> PostHistorial(HistorialDto historialDto)
        {
            var historial = _mapper.Map<Historial>(historialDto);
            historial.CreatedAt = DateTime.UtcNow;

            _context.Historiales.Add(historial);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<HistorialDto>(historial);

            return CreatedAtAction(nameof(GetHistorial), new { id = resultDto.Id }, resultDto);
        }

        // DELETE: api/Historial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorial(ulong id)
        {
            var historial = await _context.Historiales.FindAsync(id);
            if (historial == null)
                return NotFound();

            _context.Historiales.Remove(historial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorialExists(ulong id)
        {
            return _context.Historiales.Any(e => e.Id == id);
        }
    }
}
