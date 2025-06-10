using AutoMapper;
using System.Globalization;
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
        public async Task<ActionResult<EstadoDto>> GetEstado(uint id)
        {
            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            var estadoDto = _mapper.Map<EstadoDto>(estado);
            return Ok(estadoDto);
        }

        // PUT: api/Estado/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutEstado(uint id, [FromBody] EstadoActualizarDto estadoActualizarDto)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            // --- Manejo de Letra ---
            string? letra = estadoActualizarDto.Letra?.Trim().ToUpperInvariant();

            if (!string.IsNullOrEmpty(letra) && letra != estado.Letra)
            {
                var estadoLetra = await _context.Estados
                    .FirstOrDefaultAsync(e => e.Letra == letra && e.Id != id);

                if (estadoLetra != null)
                {
                    return BadRequest(new { message = $"Ya existe un estado con la letra '{letra}'. La letra debe ser única." });
                }

                estado.Letra = letra;
            }

            // --- Manejo de Descripción ---
            string? descripcion = null;
            if (!string.IsNullOrEmpty(estadoActualizarDto.Descripcion))
            {
                descripcion = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(estadoActualizarDto.Descripcion.Trim().ToLowerInvariant());
                descripcion = System.Text.RegularExpressions.Regex.Replace(descripcion, @"\s+", " ").Trim();
            }

            if (!string.IsNullOrEmpty(descripcion) && descripcion != estado.Descripcion)
            {
                estado.Descripcion = descripcion;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var estadoDto = _mapper.Map<EstadoDto>(estado);
            return Ok(estadoDto);
        }

        // POST: api/Estado
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EstadoDto>> PostEstado([FromBody] EstadoCrearDto estadoCrearDto)
        {
            string? letra = estadoCrearDto.Letra?.Trim().ToUpperInvariant();
            string? descripcion = null;
            if (!string.IsNullOrEmpty(estadoCrearDto.Descripcion))
            {
                descripcion = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(estadoCrearDto.Descripcion.Trim().ToLowerInvariant());
                descripcion = System.Text.RegularExpressions.Regex.Replace(descripcion, @"\s+", " ").Trim();
            }

            if (string.IsNullOrEmpty(letra))
            {
                return BadRequest(new { message = "La letra del estado no puede estar vacía." });
            }

            var estadoLetra = await _context.Estados.FirstOrDefaultAsync(e => e.Letra == letra);
            if (estadoLetra != null)
            {
                return BadRequest(new { message = $"Ya existe un estado con la letra '{letra}'. La letra debe ser única." });
            }

            var estado = _mapper.Map<Estado>(estadoCrearDto);

            estado.Letra = letra;
            estado.Descripcion = descripcion!;

            _context.Estados.Add(estado);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al guardar el estado. Asegúrate de que la letra sea única y que no haya otros problemas de la base de datos." });
            }

            var estadoDto = _mapper.Map<EstadoDto>(estado);

            return CreatedAtAction(nameof(GetEstado), new { id = estadoDto.Id }, estadoDto);
        }

        // DELETE: api/Estado/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEstado(uint id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(uint id)
        {
            return _context.Estados.Any(e => e.Id == id);
        }
    }
}
