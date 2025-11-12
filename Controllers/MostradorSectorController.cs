using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.MostradorSector;
using TurneroApi.Interfaces;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MostradorSectorController : ControllerBase
{
  private readonly IMostradorSectorService _service;

  public MostradorSectorController(IMostradorSectorService service)
  {
    _service = service;
  }

  // POST: api/MostradorSector
  [HttpPost]
  [Authorize(Policy = "asociar_mostrador_sector")]
  public async Task<IActionResult> Asociar([FromBody] MostradorSectorCrearDto dto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (ok, error) = await _service.AsociarAsync(dto.MostradorId, dto.SectorId);
    if (!ok) return BadRequest(new { message = error });

    return NoContent();
  }

  // DELETE: api/MostradorSector/{mostradorId}/{sectorId}
  [HttpDelete("{mostradorId}/{sectorId}")]
  [Authorize(Policy = "desasociar_mostrador_sector")]
  public async Task<IActionResult> Desasociar(int mostradorId, int sectorId)
  {
    var (ok, error) = await _service.DesasociarAsync(mostradorId, sectorId);
    if (!ok)
    {
      if (error?.Contains("no existe") == true)
        return NotFound(new { message = error });
      return BadRequest(new { message = error });
    }

    return NoContent();
  }

  // GET: api/MostradorSector/por-mostrador/{mostradorId}
  [HttpGet("por-mostrador/{mostradorId}")]
  public async Task<IActionResult> GetSectoresPorMostrador(int mostradorId)
  {
    var sectores = await _service.GetSectoresPorMostradorAsync(mostradorId);
    return Ok(sectores);
  }

  // GET: api/MostradorSector/por-sector/{sectorId}
  [HttpGet("por-sector/{sectorId}")]
  public async Task<IActionResult> GetMostradoresPorSector(int sectorId)
  {
    var mostradores = await _service.GetMostradoresPorSectorAsync(sectorId);
    return Ok(mostradores);
  }
}