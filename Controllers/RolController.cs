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
    public class RolController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public RolController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Rol
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            var dtoList = _mapper.Map<IEnumerable<RolDto>>(roles);
            return Ok(dtoList);
        }

        // GET: api/Rol/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolDto>> GetRol(ulong id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return NotFound();

            return Ok(_mapper.Map<RolDto>(rol));
        }

        // PUT: api/Rol/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(ulong id, RolDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del DTO no coincide con el ID de la URL.");

            var entity = await _context.Roles.FindAsync(id);
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
                if (!RolExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Rol
        [HttpPost]
        public async Task<ActionResult<RolDto>> PostRol(RolDto dto)
        {
            var entity = _mapper.Map<Rol>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<RolDto>(entity);
            return CreatedAtAction(nameof(GetRol), new { id = resultDto.Id }, resultDto);
        }

        // DELETE: api/Rol/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(ulong id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return NotFound();

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(ulong id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
