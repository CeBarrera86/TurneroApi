using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Permiso;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermisoController : ControllerBase
{
  private readonly IPermisoService _permisoService;
  private readonly IMapper _mapper;

  public PermisoController(IPermisoService permisoService, IMapper mapper)
  {
    _permisoService = permisoService;
    _mapper = mapper;
  }

  // GET: api/Permiso
  [HttpGet]
  public async Task<ActionResult<IEnumerable<PermisoDto>>> GetPermisos()
  {
    var permisos = await _permisoService.GetPermisosAsync();
    return Ok(_mapper.Map<IEnumerable<PermisoDto>>(permisos));
  }

  // GET: api/Permiso/5
  [HttpGet("{id}")]
  public async Task<ActionResult<PermisoDto>> GetPermiso(int id)
  {
    var permiso = await _permisoService.GetPermisoAsync(id);
    if (permiso == null) return NotFound();

    return Ok(_mapper.Map<PermisoDto>(permiso));
  }

  // POST: api/Permiso
  [HttpPost]
  [Authorize(Policy = "Permiso.Crear")]
  public async Task<ActionResult<PermisoDto>> PostPermiso([FromBody] PermisoCrearDto permisoCrearDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var permiso = _mapper.Map<Permiso>(permisoCrearDto);
    var (createdPermiso, errorMessage) = await _permisoService.CreatePermisoAsync(permiso);

    if (createdPermiso == null) return BadRequest(new { message = errorMessage });

    var permisoDto = _mapper.Map<PermisoDto>(createdPermiso);
    return CreatedAtAction(nameof(GetPermiso), new { id = permisoDto.Id }, permisoDto);
  }

  // PUT: api/Permiso/5
  [HttpPut("{id}")]
  [Authorize(Policy = "Permiso.Actualizar")]
  public async Task<ActionResult<PermisoDto>> PutPermiso(int id, [FromBody] PermisoActualizarDto permisoActualizarDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (updatedPermiso, errorMessage) = await _permisoService.UpdatePermisoAsync(id, permisoActualizarDto);
    if (updatedPermiso == null)
    {
      if (errorMessage?.Contains("no encontrado") == true)
        return NotFound(new { message = errorMessage });
      return BadRequest(new { message = errorMessage });
    }

    return Ok(_mapper.Map<PermisoDto>(updatedPermiso));
  }

  // DELETE: api/Permiso/5
  [HttpDelete("{id}")]
  [Authorize(Policy = "Permiso.Eliminar")]
  public async Task<IActionResult> DeletePermiso(int id)
  {
    var (deleted, errorMessage) = await _permisoService.DeletePermisoAsync(id);
    if (!deleted)
    {
      if (errorMessage?.Contains("no existe") == true)
        return NotFound(new { message = errorMessage });
      if (errorMessage?.Contains("asignado") == true)
        return Conflict(new { message = errorMessage });
      return StatusCode(500, new { message = errorMessage });
    }

    return NoContent();
  }
}