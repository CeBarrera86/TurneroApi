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
    public class MostradorController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public MostradorController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Mostrador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MostradorDto>>> GetMostradores()
        {
            var mostradores = await _context.Mostradores.ToListAsync();
            var dtoList = _mapper.Map<IEnumerable<MostradorDto>>(mostradores);
            return Ok(dtoList);
        }

        // GET: api/Mostrador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MostradorDto>> GetMostrador(ulong id)
        {
            var mostrador = await _context.Mostradores.FindAsync(id);

            if (mostrador == null)
                return NotFound();

            return Ok(_mapper.Map<MostradorDto>(mostrador));
        }

        // PUT: api/Mostrador/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMostrador(ulong id, MostradorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del DTO no coincide con el ID de la URL.");

            var entity = await _context.Mostradores.FindAsync(id);
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
                if (!MostradorExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Mostrador
        [HttpPost]
        public async Task<ActionResult<MostradorDto>> PostMostrador(MostradorDto dto)
        {
            var entity = _mapper.Map<Mostrador>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            _context.Mostradores.Add(entity);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<MostradorDto>(entity);
            return CreatedAtAction(nameof(GetMostrador), new { id = resultDto.Id }, resultDto);
        }

        // DELETE: api/Mostrador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMostrador(ulong id)
        {
            var entity = await _context.Mostradores.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Mostradores.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MostradorExists(ulong id)
        {
            return _context.Mostradores.Any(e => e.Id == id);
        }
    }
}
