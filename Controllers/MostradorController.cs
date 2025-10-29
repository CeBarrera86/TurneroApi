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
    public async Task<ActionResult<IEnumerable<MostradorDto>>> GetMostradores()
    {
      var mostradores = await _mostradorService.GetMostradoresAsync();
      var mostradoresDto = _mapper.Map<IEnumerable<MostradorDto>>(mostradores);
      return Ok(mostradoresDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MostradorDto>> GetMostrador(uint id)
    {
      var mostrador = await _mostradorService.GetMostradorAsync(id);
      if (mostrador == null) return NotFound();

      var mostradorDto = _mapper.Map<MostradorDto>(mostrador);
      return Ok(mostradorDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutMostrador(uint id, [FromBody] MostradorActualizarDto dto)
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMostrador(uint id)
    {
      var deleted = await _mostradorService.DeleteMostradorAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}