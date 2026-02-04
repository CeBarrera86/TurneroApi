using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Sector;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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
  [Authorize(Policy = "ver_sector")]
  public async Task<ActionResult<PagedResponse<SectorDto>>> GetSectores([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _sectorService.GetSectoresAsync(page, pageSize);
    return Ok(new PagedResponse<SectorDto>(result.Items, page, pageSize, result.Total));
  }

  // GET: api/Sector
  [HttpGet("totem-sectores")]
  [Authorize(Policy = "TotemAccess")]
  public async Task<ActionResult<PagedResponse<SectorDto>>> GetSectoresTotem([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _sectorService.GetSectoresTotemAsync(page, pageSize);
    return Ok(new PagedResponse<SectorDto>(result.Items, page, pageSize, result.Total));

  }

  // GET: api/Sector
  [HttpGet("totem-tramites")]
  [Authorize(Policy = "TotemAccess")]
  public async Task<ActionResult<PagedResponse<SectorDto>>> GetTramitesTotem([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _sectorService.GetTramitesTotemAsync(page, pageSize);
    return Ok(new PagedResponse<SectorDto>(result.Items, page, pageSize, result.Total));
  }

  // GET: api/Sector/5
  [HttpGet("{id}")]
  [Authorize(Policy = "ver_sector")]
  public async Task<ActionResult<SectorDto>> GetSector(int id)
  {
    var sector = await _sectorService.GetSectorAsync(id);
    if (sector == null) return NotFound();
    return Ok(sector);
  }

  // POST: api/Sector
  [HttpPost]
  [Authorize(Policy = "crear_sector")]
  public async Task<ActionResult<SectorDto>> PostSector([FromBody] SectorCrearDto sectorCrearDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var sector = _mapper.Map<Sector>(sectorCrearDto);
    var (createdSector, errorMessage) = await _sectorService.CreateSectorAsync(sector);

    if (createdSector == null) return BadRequest(new { message = errorMessage });

    var sectorDto = _mapper.Map<SectorDto>(createdSector);
    return CreatedAtAction(nameof(GetSector), new { id = sectorDto.Id }, sectorDto);
  }

  // PUT: api/Sector/5
  [HttpPut("{id}")]
  [Authorize(Policy = "editar_sector")]
  public async Task<ActionResult<SectorDto>> PutSector(int id, [FromBody] SectorActualizarDto sectorActualizarDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (updatedSector, errorMessage) = await _sectorService.UpdateSectorAsync(id, sectorActualizarDto);
    if (updatedSector == null)
    {
      if (errorMessage?.Contains("no encontrado") == true)
        return NotFound(new { message = errorMessage });
      return BadRequest(new { message = errorMessage });
    }

    var sectorDto = _mapper.Map<SectorDto>(updatedSector);
    return Ok(sectorDto);
  }

  // GET: api/Sector/activos
  [HttpGet("activos")]
  [Authorize(Policy = "ver_sector")]
  public async Task<ActionResult<PagedResponse<SectorDto>>> GetSectoresActivos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _sectorService.GetSectoresActivosAsync(page, pageSize);
    return Ok(new PagedResponse<SectorDto>(result.Items, page, pageSize, result.Total));
  }

  // GET: api/Sector/activos-padres
  [HttpGet("activos-padres")]
  [Authorize(Policy = "ver_sector")]
  public async Task<ActionResult<PagedResponse<SectorDto>>> GetSectoresActivosPadres([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _sectorService.GetSectoresActivosPadresAsync(page, pageSize);
    return Ok(new PagedResponse<SectorDto>(result.Items, page, pageSize, result.Total));
  }

  // DELETE: api/Sector/5
  [HttpDelete("{id}")]
  [Authorize(Policy = "eliminar_sector")]
  public async Task<IActionResult> DeleteSector(int id)
  {
    var (deleted, errorMessage) = await _sectorService.DeleteSectorAsync(id);
    if (!deleted) return BadRequest(new { message = errorMessage });
    return NoContent();
  }
}