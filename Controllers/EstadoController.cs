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
    public class EstadoController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public EstadoController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Estado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoDto>>> GetEstados()
        {
            var estados = await _context.Estados.ToListAsync();
            var estadosDto = _mapper.Map<IEnumerable<EstadoDto>>(estados);
            return Ok(estadosDto);
        }

        // GET: api/Estado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoDto>> GetEstado(ulong id)
        {
            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
                return NotFound();

            return Ok(_mapper.Map<EstadoDto>(estado));
        }

        // PUT: api/Estado/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado(ulong id, EstadoDto estadoDto)
        {
            if (id != estadoDto.Id)
                return BadRequest("ID de la URL y el objeto no coinciden.");

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
                return NotFound();

            _mapper.Map(estadoDto, estado);
            estado.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Estado
        [HttpPost]
        public async Task<ActionResult<EstadoDto>> PostEstado(EstadoDto estadoDto)
        {
            var estado = _mapper.Map<Estado>(estadoDto);
            estado.CreatedAt = DateTime.UtcNow;

            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<EstadoDto>(estado);

            return CreatedAtAction(nameof(GetEstado), new { id = estado.Id }, resultDto);
        }

        // DELETE: api/Estado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado(ulong id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
                return NotFound();

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(ulong id)
        {
            return _context.Estados.Any(e => e.Id == id);
        }
    }
}
