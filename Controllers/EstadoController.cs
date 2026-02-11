using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Estado;
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
    [Authorize(Policy = "ver_estado")]
    public async Task<ActionResult<IEnumerable<EstadoDto>>> GetEstados()
    {
      var result = await _estadoService.GetEstadosAsync();
      return Ok(result);
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]
    [Authorize(Policy = "ver_estado")]
    public async Task<ActionResult<EstadoDto>> GetEstado(int id)
    {
      var estado = await _estadoService.GetEstadoAsync(id);
      if (estado == null)
      {
        return NotFound();
      }
      return Ok(estado);
    }

    // PUT: api/Estado/5
    [HttpPut("{id}")]
    [Authorize(Policy = "editar_estado")]
    public async Task<IActionResult> PutEstado(int id, [FromBody] EstadoActualizarDto estadoActualizarDto)
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
    [Authorize(Policy = "crear_estado")]
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
    [Authorize(Policy = "eliminar_estado")]
    public async Task<IActionResult> DeleteEstado(int id)
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