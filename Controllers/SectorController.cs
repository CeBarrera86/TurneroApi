using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SectorController : ControllerBase
    {
        private readonly ISectorService _sectorService;
        private readonly IMapper _mapper;

        public SectorController(ISectorService sectorService, IMapper mapper)
        {
            _sectorService = sectorService;
            _mapper = mapper;
        }

        // GET: api/Sector
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectorDto>>> GetSectores()
        {
            var sectores = await _sectorService.GetSectoresAsync();
            var sectoresDto = _mapper.Map<IEnumerable<SectorDto>>(sectores);
            return Ok(sectoresDto);
        }

        // GET: api/Sector/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectorDto>> GetSector(uint id)
        {
            var sector = await _sectorService.GetSectorAsync(id);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (updatedSector, errorMessage) = await _sectorService.UpdateSectorAsync(id, sectorActualizarDto);

            if (updatedSector == null)
            {
                if (errorMessage == "Sector no encontrado." || errorMessage == "Sector no encontrado (error de concurrencia).")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            var sectorDto = _mapper.Map<SectorDto>(updatedSector);
            return Ok(sectorDto);
        }

        // POST: api/Sector
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SectorDto>> PostSector([FromBody] SectorCrearDto sectorCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sector = _mapper.Map<Sector>(sectorCrearDto);

            var (createdSector, errorMessage) = await _sectorService.CreateSectorAsync(sector);

            if (createdSector == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var sectorDto = _mapper.Map<SectorDto>(createdSector);

            return CreatedAtAction(nameof(GetSector), new { id = sectorDto.Id }, sectorDto);
        }

        // DELETE: api/Sector/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSector(uint id)
        {
            var deleted = await _sectorService.DeleteSectorAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "No se pudo eliminar el sector. Puede que no exista o tenga elementos asociados." });
            }
            return NoContent();
        }
    }
}