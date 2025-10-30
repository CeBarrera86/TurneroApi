using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContenidoController : ControllerBase
{
  private readonly IContenidoService _service;
  private readonly IMapper _mapper;

  public ContenidoController(IContenidoService service, IMapper mapper)
  {
    _service = service;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<ContenidoDto>>> GetContenidos()
  {
    var contenidos = await _service.GetContenidosAsync();
    return Ok(_mapper.Map<IEnumerable<ContenidoDto>>(contenidos));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ContenidoDto>> GetContenido(uint id)
  {
    var contenido = await _service.GetContenidoAsync(id);
    if (contenido == null) return NotFound();
    return Ok(_mapper.Map<ContenidoDto>(contenido));
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<IEnumerable<ContenidoDto>>> PostContenidos([FromForm] List<IFormFile> archivos, [FromForm] List<bool> activos)
  {
    var (contenidos, error) = await _service.CreateContenidosAsync(archivos, activos);
    if (error != null) return BadRequest(new { message = error });

    var dtos = _mapper.Map<IEnumerable<ContenidoDto>>(contenidos);
    return Ok(dtos);
  }

  [HttpPut("{id}")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<ContenidoDto>> PutContenido(uint id, [FromBody] ContenidoActualizarDto dto)
  {
    var (contenido, error) = await _service.UpdateContenidoAsync(id, dto);
    if (contenido == null) return NotFound(new { message = error });

    return Ok(_mapper.Map<ContenidoDto>(contenido));
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> DeleteContenido(uint id)
  {
    var (deleted, error) = await _service.DeleteContenidoAsync(id);
    if (!deleted) return NotFound(new { message = error });
    return NoContent();
  }
}