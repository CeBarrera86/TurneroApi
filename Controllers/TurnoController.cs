using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Turno;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TurnoController : ControllerBase
  {
    private readonly ITurnoService _turnoService;
    private readonly IMapper _mapper;

    public TurnoController(ITurnoService turnoService, IMapper mapper)
    {
      _turnoService = turnoService;
      _mapper = mapper;
    }

    // GET: api/Turno?page=1&pageSize=10
    [HttpGet]
    [Authorize(Policy = "ver_turno")]
    public async Task<ActionResult<PagedResponse<TurnoDto>>> GetTurnos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (!PaginationHelper.IsValid(page, pageSize, out var message))
      {
        return BadRequest(new { message });
      }

      var result = await _turnoService.GetTurnosAsync(page, pageSize);
      return Ok(new PagedResponse<TurnoDto>(result.Items, page, pageSize, result.Total));
    }

    // GET: api/Turno/5
    [HttpGet("{id}")]
    [Authorize(Policy = "ver_turno")]
    public async Task<ActionResult<TurnoDto>> GetTurno(ulong id)
    {
      var turno = await _turnoService.GetTurnoAsync(id);

      if (turno == null)
      {
        return NotFound();
      }

      return Ok(turno);
    }

    // GET: api/Turno/puesto/{puestoId}/activo
    // Nuevo endpoint para obtener el turno activo de un puesto (si lo necesitas)
    [HttpGet("puesto/{puestoId}/activo")]
    [Authorize(Policy = "ver_turno")]
    public async Task<ActionResult<TurnoDto>> GetTurnoActivoPorPuesto(int puestoId) // ← int en lugar de uint
    {
      var turno = await _turnoService.GetTurnoActivoPorPuestoIdAsync(puestoId);
      if (turno == null)
      {
        return NotFound($"No se encontró ningún turno activo para el puesto '{puestoId}'.");
      }
      return Ok(turno);
    }

    // POST: api/Turno
    [HttpPost]
    [Authorize(Policy = "crear_turno")]
    public async Task<ActionResult<TurnoDto>> PostTurno([FromBody] TurnoCrearDto turnoCrearDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var (createdTurno, errorMessage) = await _turnoService.CreateTurnoAsync(turnoCrearDto);

      if (createdTurno == null)
      {
        return BadRequest(new { message = errorMessage });
      }

      var turnoDto = _mapper.Map<TurnoDto>(createdTurno);
      // CreatedAtAction se refiere al método GET para un solo Turno
      return CreatedAtAction(nameof(GetTurno), new { id = turnoDto.Id }, turnoDto);
    }

    // PATCH: api/Turno/5 (Para actualizar FechaFin o EstadoId)
    // Usamos PATCH porque solo actualizamos parcialmente el recurso.
    [HttpPatch("{id}")]
    [Authorize(Policy = "editar_turno")]
    public async Task<IActionResult> PatchTurno(ulong id, [FromBody] TurnoActualizarDto turnoActualizarDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var (updatedTurno, errorMessage) = await _turnoService.UpdateTurnoAsync(id, turnoActualizarDto);

      if (updatedTurno == null)
      {
        if (errorMessage == "Turno no encontrado.")
        {
          return NotFound(new { message = errorMessage });
        }
        return BadRequest(new { message = errorMessage });
      }

      var turnoDto = _mapper.Map<TurnoDto>(updatedTurno);
      return Ok(turnoDto); // Devuelve el Turno actualizado
    }
  }
}