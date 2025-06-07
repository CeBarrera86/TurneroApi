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
    public class TareaController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public TareaController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Tarea
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDto>>> GetTareas()
        {
            var tareas = await _context.Tareas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TareaDto>>(tareas));
        }

        // GET: api/Tarea/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDto>> GetTarea(ulong id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound();

            return Ok(_mapper.Map<TareaDto>(tarea));
        }

        // PUT: api/Tarea/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(ulong id, TareaDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el del DTO.");

            var entity = await _context.Tareas.FindAsync(id);
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
                if (!TareaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Tarea
        [HttpPost]
        public async Task<ActionResult<TareaDto>> PostTarea(TareaDto dto)
        {
            var entity = _mapper.Map<Tarea>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            _context.Tareas.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = entity.Id }, _mapper.Map<TareaDto>(entity));
        }

        // DELETE: api/Tarea/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(ulong id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound();

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(ulong id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
