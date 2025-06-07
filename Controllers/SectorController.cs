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
    public class SectorController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public SectorController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Sector
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectorDto>>> GetSectores()
        {
            var sectores = await _context.Sectores.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SectorDto>>(sectores));
        }

        // GET: api/Sector/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectorDto>> GetSector(ulong id)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
                return NotFound();

            return Ok(_mapper.Map<SectorDto>(sector));
        }

        // PUT: api/Sector/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSector(ulong id, SectorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID proporcionado no coincide con el del cuerpo.");

            var entity = await _context.Sectores.FindAsync(id);
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
                if (!SectorExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Sector
        [HttpPost]
        public async Task<ActionResult<SectorDto>> PostSector(SectorDto dto)
        {
            var entity = _mapper.Map<Sector>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            _context.Sectores.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSector), new { id = entity.Id }, _mapper.Map<SectorDto>(entity));
        }

        // DELETE: api/Sector/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(ulong id)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
                return NotFound();

            _context.Sectores.Remove(sector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SectorExists(ulong id)
        {
            return _context.Sectores.Any(e => e.Id == id);
        }
    }
}
