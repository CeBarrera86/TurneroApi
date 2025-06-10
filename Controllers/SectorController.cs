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
            var sectores = await _context.Sectores
                .Include(s => s.Padre)
                .ToListAsync();
            var sectoresDto = _mapper.Map<IEnumerable<SectorDto>>(sectores);
            return Ok(sectoresDto);
        }

        // GET: api/Sector/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectorDto>> GetSector(uint id)
        {
            var sector = await _context.Sectores
                .Include(s => s.Padre)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sector == null)
            {
                return NotFound();
            }

            var sectorDto = _mapper.Map<SectorDto>(sector);
            return Ok(sectorDto);
        }

        // PUT: api/Sector/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSector(uint id, [FromBody] SectorActualizarDto sectorActualizarDto)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
            {
                return NotFound();
            }

            // --- Manejo y Normalización ---
            string? letra = null;
            string? nombre = null;
            string? descripcion = null;

            if (!string.IsNullOrEmpty(sectorActualizarDto.Letra))
            {
                letra = sectorActualizarDto.Letra.Trim().Replace(" ", "").ToUpperInvariant();
            }
            if (!string.IsNullOrEmpty(letra) && letra != sector.Letra)
            {
                var letraExistente = await _context.Sectores
                    .FirstOrDefaultAsync(s => s.Letra == letra && s.Id != id);
                if (letraExistente != null)
                {
                    return BadRequest(new { message = $"La letra '{letra}' ya está en uso por otro sector. Debe ser única." });
                }
                sector.Letra = letra;
            }

            if (!string.IsNullOrEmpty(sectorActualizarDto.Nombre))
            {
                nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sectorActualizarDto.Nombre.Trim().ToLowerInvariant());
                nombre = Regex.Replace(nombre, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(nombre) && nombre != sector.Nombre)
            {
                var nombreExistente = await _context.Sectores
                    .FirstOrDefaultAsync(s => s.Nombre == nombre && s.Id != id);

                if (nombreExistente != null)
                {
                    return BadRequest(new { message = $"El nombre '{nombre}' ya está en uso por otro sector. Debe ser único." });
                }
                sector.Nombre = nombre;
            }

            
            if (!string.IsNullOrEmpty(sectorActualizarDto.Descripcion))
            {
                descripcion = sectorActualizarDto.Descripcion.Trim().Replace(" ", "").ToUpperInvariant();
            }
            if (!string.IsNullOrEmpty(descripcion) && descripcion != sector.Descripcion)
            {
                sector.Descripcion = descripcion;
            }

            if (sectorActualizarDto.PadreId.HasValue && sectorActualizarDto.PadreId.Value != sector.PadreId)
            {
                if (sectorActualizarDto.PadreId.Value != id)
                {
                    var padreExiste = await _context.Sectores.AnyAsync(s => s.Id == sectorActualizarDto.PadreId.Value);
                    if (padreExiste)
                    {
                        sector.PadreId = sectorActualizarDto.PadreId.Value;
                    }
                    else
                    {
                        return BadRequest(new { message = $"El PadreId '{sectorActualizarDto.PadreId.Value}' proporcionado no es válido." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Un sector no puede ser su propio padre." });
                }
            }
            else if (!sectorActualizarDto.PadreId.HasValue && sector.PadreId.HasValue)
            {
                sector.PadreId = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectorExists(id))
                    return NotFound();
                throw;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al actualizar el sector. Asegúrate de que la letra y el nombre sean únicos." });
            }

            await _context.Entry(sector).Reference(s => s.Padre).LoadAsync();
            var sectorDto = _mapper.Map<SectorDto>(sector);
            return Ok(sectorDto);
        }

        // POST: api/Sector
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SectorDto>> PostSector([FromBody] SectorCrearDto sectorCrearDto)
        {
            // --- Limpieza y Normalización ---
            string? letra = null;
            string? nombre = null;
            string? descripcion = null;

            if (!string.IsNullOrEmpty(sectorCrearDto.Letra))
            {
                letra = sectorCrearDto.Letra.Trim().Replace(" ", "").ToUpperInvariant();
            }

            if (!string.IsNullOrEmpty(sectorCrearDto.Nombre))
            {
                nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sectorCrearDto.Nombre.Trim().ToLowerInvariant());
                nombre = Regex.Replace(nombre, @"\s+", " ").Trim();
            }

            if (!string.IsNullOrEmpty(sectorCrearDto.Descripcion))
            {
                descripcion = sectorCrearDto.Descripcion.Trim().Replace(" ", "").ToUpperInvariant();
            }

            // Validaciones
            if (!string.IsNullOrEmpty(letra))
            {
                var letraExistente = await _context.Sectores
                    .FirstOrDefaultAsync(s => s.Letra == letra);
                if (letraExistente != null)
                {
                    return BadRequest(new { message = $"La letra '{letra}' ya está en uso. Debe ser única." });
                }
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                var nombreExistente = await _context.Sectores
                    .FirstOrDefaultAsync(s => s.Nombre == nombre);
                if (nombreExistente != null)
                {
                    return BadRequest(new { message = $"El nombre '{nombre}' ya está en uso. Debe ser único." });
                }
            }

            if (sectorCrearDto.PadreId.HasValue)
            {
                var padreExiste = await _context.Sectores.AnyAsync(s => s.Id == sectorCrearDto.PadreId.Value);
                if (!padreExiste)
                {
                    return BadRequest(new { message = $"El PadreId '{sectorCrearDto.PadreId.Value}' proporcionado no es válido." });
                }
            }

            var sector = _mapper.Map<Sector>(sectorCrearDto);

            sector.Letra = letra;
            sector.Nombre = nombre;
            sector.Descripcion = descripcion;
            sector.PadreId = sectorCrearDto.PadreId;

            _context.Sectores.Add(sector);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al guardar el sector. Asegúrate de que la letra y el nombre sean únicos." });
            }

            await _context.Entry(sector).Reference(s => s.Padre).LoadAsync();
            var sectorDto = _mapper.Map<SectorDto>(sector);

            return CreatedAtAction(nameof(GetSector), new { id = sectorDto.Id }, sectorDto);
        }

        // DELETE: api/Sector/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(uint id)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
            {
                return NotFound();
            }

            _context.Sectores.Remove(sector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SectorExists(uint id)
        {
            return _context.Sectores.Any(e => e.Id == id);
        }
    }
}
