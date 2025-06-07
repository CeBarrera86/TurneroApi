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
    public class PuestoController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public PuestoController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Puesto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PuestoDto>>> GetPuestos()
        {
            var puestos = await _context.Puestos.ToListAsync();
            var dtoList = _mapper.Map<IEnumerable<PuestoDto>>(puestos);
            return Ok(dtoList);
        }

        // GET: api/Puesto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PuestoDto>> GetPuesto(ulong id)
        {
            var puesto = await _context.Puestos.FindAsync(id);

            if (puesto == null)
                return NotFound();

            return Ok(_mapper.Map<PuestoDto>(puesto));
        }

        // PUT: api/Puesto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuesto(ulong id, PuestoDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del DTO no coincide con el ID de la URL.");

            var entity = await _context.Puestos.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);
            entity.Logout = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuestoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Puesto
        [HttpPost]
        public async Task<ActionResult<PuestoDto>> PostPuesto(PuestoDto dto)
        {
            var entity = _mapper.Map<Puesto>(dto);
            entity.Login = DateTime.UtcNow;

            _context.Puestos.Add(entity);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<PuestoDto>(entity);
            return CreatedAtAction(nameof(GetPuesto), new { id = resultDto.Id }, resultDto);
        }

        // DELETE: api/Puesto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuesto(ulong id)
        {
            var entity = await _context.Puestos.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Puestos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuestoExists(ulong id)
        {
            return _context.Puestos.Any(e => e.Id == id);
        }
    }
}