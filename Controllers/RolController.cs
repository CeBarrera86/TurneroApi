using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Rol;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolController : ControllerBase
{
  private readonly IRolService _rolService;
  private readonly IMapper _mapper;

  public RolController(IRolService rolService, IMapper mapper)
  {
    _rolService = rolService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
  {
    var roles = await _rolService.GetRolesAsync();
    return Ok(_mapper.Map<IEnumerable<RolDto>>(roles));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<RolDto>> GetRol(int id)
  {
    var rol = await _rolService.GetRolAsync(id);
    if (rol == null) return NotFound();
    return Ok(_mapper.Map<RolDto>(rol));
  }

  [HttpPost]
  [Authorize(Policy = "Rol.Crear")]
  public async Task<ActionResult<RolDto>> PostRol([FromBody] RolCrearDto rolCrearDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var rol = _mapper.Map<Rol>(rolCrearDto);
    var (createdRol, errorMessage) = await _rolService.CreateRolAsync(rol);

    if (createdRol == null) return BadRequest(new { message = errorMessage });

    var rolDto = _mapper.Map<RolDto>(createdRol);
    return CreatedAtAction(nameof(GetRol), new { id = rolDto.Id }, rolDto);
  }

  [HttpPut("{id}")]
  [Authorize(Policy = "Rol.Actualizar")]
  public async Task<ActionResult<RolDto>> PutRol(int id, [FromBody] RolActualizarDto rolActualizarDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (updatedRol, errorMessage) = await _rolService.UpdateRolAsync(id, rolActualizarDto);
    if (updatedRol == null)
    {
      if (errorMessage?.Contains("no encontrado") == true)
        return NotFound(new { message = errorMessage });
      return BadRequest(new { message = errorMessage });
    }

    return Ok(_mapper.Map<RolDto>(updatedRol));
  }

  [HttpDelete("{id}")]
  [Authorize(Policy = "Rol.Eliminar")]
  public async Task<IActionResult> DeleteRol(int id)
  {
    var (deleted, errorMessage) = await _rolService.DeleteRolAsync(id);
    if (!deleted)
    {
      if (errorMessage?.Contains("asignado") == true)
        return Conflict(new { message = errorMessage });
      if (errorMessage?.Contains("no existe") == true)
        return NotFound(new { message = errorMessage });
      return StatusCode(500, new { message = errorMessage });
    }
    return NoContent();
  }
}