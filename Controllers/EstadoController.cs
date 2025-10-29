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
  public class EstadoController : ControllerBase
  {
    private readonly IEstadoService _estadoService;
    private readonly IMapper _mapper;

    public EstadoController(IEstadoService estadoService, IMapper mapper)
    {
      _estadoService = estadoService;
      _mapper = mapper;
    }

    // GET: api/Estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoDto>>> GetEstados()
    {
      var estados = await _estadoService.GetEstadosAsync();
      var estadosDto = _mapper.Map<IEnumerable<EstadoDto>>(estados);
      return Ok(estadosDto);
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EstadoDto>> GetEstado(uint id)
    {
      var estado = await _estadoService.GetEstadoAsync(id);
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
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var (updatedEstado, errorMessage) = await _estadoService.UpdateEstadoAsync(id, estadoActualizarDto);

      if (updatedEstado == null)
      {
        if (errorMessage == "Estado no encontrado." || errorMessage == "Estado no encontrado (error de concurrencia).")
        {
          return NotFound(new { message = errorMessage });
        }
        return BadRequest(new { message = errorMessage });
      }

      var estadoDto = _mapper.Map<EstadoDto>(updatedEstado);
      return Ok(estadoDto);
    }

    // POST: api/Estado
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EstadoDto>> PostEstado([FromBody] EstadoCrearDto estadoCrearDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var estado = _mapper.Map<Estado>(estadoCrearDto);

      var (createdEstado, errorMessage) = await _estadoService.CreateEstadoAsync(estado);

      if (createdEstado == null)
      {
        return BadRequest(new { message = errorMessage });
      }

      var estadoDto = _mapper.Map<EstadoDto>(createdEstado);

      return CreatedAtAction(nameof(GetEstado), new { id = estadoDto.Id }, estadoDto);
    }

    // DELETE: api/Estado/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEstado(uint id)
    {
      var (deleted, errorMessage) = await _estadoService.DeleteEstadoAsync(id);
      if (!deleted)
      {
        return NotFound(new { message = errorMessage });
      }
      return NoContent();
    }
  }
}