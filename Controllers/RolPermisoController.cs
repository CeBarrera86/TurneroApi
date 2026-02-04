using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.RolPermiso;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolPermisoController : ControllerBase
{
  private readonly IRolPermisoService _service;
  private readonly IMapper _mapper;

  public RolPermisoController(IRolPermisoService service, IMapper mapper)
  {
    _service = service;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<PagedResponse<RolPermisoDto>>> GetRolPermisos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _service.GetRolPermisosAsync(page, pageSize);
    return Ok(new PagedResponse<RolPermisoDto>(result.Items, page, pageSize, result.Total));
  }

  [HttpGet("{rolId}/{permisoId}")]
  public async Task<ActionResult<RolPermisoDto>> GetRolPermiso(int rolId, int permisoId)
  {
    var rolPermiso = await _service.GetRolPermisoAsync(rolId, permisoId);
    if (rolPermiso == null) return NotFound();

    return Ok(rolPermiso);
  }

  [HttpPost]
  [Authorize(Policy = "RolPermiso.Crear")]
  public async Task<ActionResult<RolPermisoDto>> PostRolPermiso([FromBody] RolPermisoCrearDto dto)
  {
    var (created, errorMessage) = await _service.CreateRolPermisoAsync(dto);
    if (created == null) return BadRequest(new { message = errorMessage });

    var dtoResult = _mapper.Map<RolPermisoDto>(created);
    return CreatedAtAction(nameof(GetRolPermiso), new { rolId = dtoResult.RolId, permisoId = dtoResult.PermisoId }, dtoResult);
  }

  [HttpDelete("{rolId}/{permisoId}")]
  [Authorize(Policy = "RolPermiso.Eliminar")]
  public async Task<IActionResult> DeleteRolPermiso(int rolId, int permisoId)
  {
    var (deleted, errorMessage) = await _service.DeleteRolPermisoAsync(rolId, permisoId);
    if (!deleted)
    {
      if (errorMessage == "La relación rol-permiso no existe.")
        return NotFound(new { message = errorMessage });
      return BadRequest(new { message = errorMessage });
    }

    return NoContent();
  }
}