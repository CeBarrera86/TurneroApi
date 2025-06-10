using AutoMapper;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
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
            var rolesDto = _mapper.Map<IEnumerable<RolDto>>(roles);
            return Ok(rolesDto);
        }

        // GET: api/Rol/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolDto>> GetRol(uint id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            var rolDto = _mapper.Map<RolDto>(rol);
            return Ok(rolDto);
        }

        // PUT: api/Rol/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRol(uint id, [FromBody] RolActualizarDto rolActualizarDto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            // --- Manejo y Normalización ---
            string? tipo = null;
            if (!string.IsNullOrEmpty(rolActualizarDto.Tipo))
            {
                tipo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(rolActualizarDto.Tipo.Trim().Replace(" ", "").ToLowerInvariant());
            }

            if (!string.IsNullOrEmpty(tipo) && tipo != rol.Tipo)
            {
                var tipoExistente = await _context.Roles.FirstOrDefaultAsync(r => r.Tipo == tipo && r.Id != id);
                if (tipoExistente != null)
                {
                    return BadRequest(new { message = $"El tipo de rol '{tipo}' ya está en uso por otro rol. Debe ser único." });
                }
                rol.Tipo = tipo;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al actualizar el rol. Asegúrate de que el tipo de rol sea único." });
            }

            var rolDto = _mapper.Map<RolDto>(rol);
            return Ok(rolDto);
        }

        // POST: api/Rol
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RolDto>> PostRol([FromBody] RolCrearDto rolCrearDto) // Recibe RolCrearDto y devuelve RolDto
        {
            // --- Limpieza y Normalización ---
            string? tipo = null;
            if (!string.IsNullOrEmpty(rolCrearDto.Tipo))
            {
                tipo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(rolCrearDto.Tipo.Trim().Replace(" ", "").ToLowerInvariant());
            }

            // Validación de campo obligatorio
            if (string.IsNullOrEmpty(tipo))
            {
                return BadRequest(new { message = "El tipo de rol no puede estar vacío." });
            }

            // Validación de unicidad para Tipo
            var rolExistente = await _context.Roles.FirstOrDefaultAsync(r => r.Tipo == tipo);
            if (rolExistente != null)
            {
                return BadRequest(new { message = $"El tipo de rol '{tipo}' ya está en uso. Debe ser único." });
            }

            var rol = _mapper.Map<Rol>(rolCrearDto);

            rol.Tipo = tipo!;

            _context.Roles.Add(rol);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al guardar el rol. Asegúrate de que el tipo de rol sea único." });
            }

            var rolDto = _mapper.Map<RolDto>(rol);

            return CreatedAtAction(nameof(GetRol), new { id = rolDto.Id }, rolDto);
        }

        // DELETE: api/Rol/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(uint id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(uint id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
