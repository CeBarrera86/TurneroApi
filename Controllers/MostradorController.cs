using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Mostrador;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MostradorController : ControllerBase
{
  private readonly IMostradorService _mostradorService;
  private readonly IMapper _mapper;

  public MostradorController(IMostradorService mostradorService, IMapper mapper)
  {
    _mostradorService = mostradorService;
    _mapper = mapper;
  }

  [HttpGet]
  [Authorize(Policy = "ver_mostrador")]
  public async Task<ActionResult<IEnumerable<MostradorDto>>> GetMostradores()
  {
    var result = await _mostradorService.GetMostradoresAsync();
    return Ok(result);
  }

  [HttpGet("{id}")]
  [Authorize(Policy = "ver_mostrador")]
  public async Task<ActionResult<MostradorDto>> GetMostrador(int id)
  {
    var mostrador = await _mostradorService.GetMostradorAsync(id);
    if (mostrador == null) return NotFound();
    return Ok(mostrador);
  }

  [HttpPost]
  [Authorize(Policy = "crear_mostrador")]
  public async Task<ActionResult<MostradorDto>> PostMostrador([FromBody] MostradorCrearDto dto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var mostrador = _mapper.Map<Mostrador>(dto);
    var (created, error) = await _mostradorService.CreateMostradorAsync(mostrador);

    if (created == null) return BadRequest(new { message = error });

    var dtoOut = _mapper.Map<MostradorDto>(created);
    return CreatedAtAction(nameof(GetMostrador), new { id = dtoOut.Id }, dtoOut);
  }

  [HttpPut("{id}")]
  [Authorize(Policy = "editar_mostrador")]
  public async Task<IActionResult> PutMostrador(int id, [FromBody] MostradorActualizarDto dto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (updated, error) = await _mostradorService.UpdateMostradorAsync(id, dto);

    if (updated == null)
    {
      if (error?.Contains("no encontrado") == true) return NotFound(new { message = error });
      return BadRequest(new { message = error });
    }

    var dtoOut = _mapper.Map<MostradorDto>(updated);
    return Ok(dtoOut);
  }

  [HttpDelete("{id}")]
  [Authorize(Policy = "eliminar_mostrador")]
  public async Task<IActionResult> DeleteMostrador(int id)
  {
    var deleted = await _mostradorService.DeleteMostradorAsync(id);
    if (!deleted) return NotFound();
    return NoContent();
  }
}