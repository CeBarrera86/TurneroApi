using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers
{
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

    // GET: api/Rol
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
    {
      var roles = await _rolService.GetRolesAsync();
      var rolesDto = _mapper.Map<IEnumerable<RolDto>>(roles);
      return Ok(rolesDto);
    }

    // GET: api/Rol/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RolDto>> GetRol(uint id)
    {
      var rol = await _rolService.GetRolAsync(id);
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
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var (updatedRol, errorMessage) = await _rolService.UpdateRolAsync(id, rolActualizarDto);

      if (updatedRol == null)
      {
        if (errorMessage == "Rol no encontrado." || errorMessage == "Rol no encontrado (error de concurrencia).")
        {
          return NotFound(new { message = errorMessage });
        }
        return BadRequest(new { message = errorMessage });
      }

      var rolDto = _mapper.Map<RolDto>(updatedRol);
      return Ok(rolDto);
    }

    // POST: api/Rol
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RolDto>> PostRol([FromBody] RolCrearDto rolCrearDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var rol = _mapper.Map<Rol>(rolCrearDto);

      var (createdRol, errorMessage) = await _rolService.CreateRolAsync(rol);

      if (createdRol == null)
      {
        return BadRequest(new { message = errorMessage });
      }

      var rolDto = _mapper.Map<RolDto>(createdRol);

      return CreatedAtAction(nameof(GetRol), new { id = rolDto.Id }, rolDto);
    }

    // DELETE: api/Rol/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRol(uint id)
    {
      var (deleted, errorMessage) = await _rolService.DeleteRolAsync(id);
      if (!deleted)
      {
        if (errorMessage == "No se puede eliminar el rol porque está asignado a una persona.")
          return Conflict(new { mensaje = errorMessage });

        if (errorMessage == "El rol no existe.")
          return NotFound(new { mensaje = errorMessage });

        return StatusCode(500, new { mensaje = errorMessage });
      }

      return NoContent();
    }
  }
}