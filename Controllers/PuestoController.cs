using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Puesto;
using TurneroApi.Interfaces;

namespace TurneroApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PuestoController : ControllerBase
  {
    private readonly IPuestoService _puestoService;
    private readonly IMapper _mapper;
    private readonly ILogger<PuestoController> _logger;

    public PuestoController(IPuestoService puestoService, IMapper mapper, ILogger<PuestoController> logger)
    {
      _puestoService = puestoService;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ver_puesto")]
    public async Task<ActionResult<PuestoDto>> GetPuesto(int id)
    {
      var puesto = await _puestoService.GetPuestoAsync(id);
      if (puesto == null)
        return NotFound();

      var puestoDto = _mapper.Map<PuestoDto>(puesto);
      return Ok(puestoDto);
    }

    [HttpPost]
    [Authorize(Policy = "crear_puesto")]
    public async Task<ActionResult<PuestoDto>> PostPuesto([FromBody] PuestoCrearDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var (createdPuesto, errorMessage) = await _puestoService.CreatePuestoAsync(dto);
      if (createdPuesto == null)
        return BadRequest(new { message = errorMessage });

      var puestoDto = _mapper.Map<PuestoDto>(createdPuesto);
      return CreatedAtAction(nameof(GetPuesto), new { id = puestoDto.Id }, puestoDto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "editar_puesto")]
    public async Task<IActionResult> PutPuesto(int id, [FromBody] PuestoActualizarDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var (updatedPuesto, errorMessage) = await _puestoService.UpdatePuestoAsync(id, dto);
      if (updatedPuesto == null)
      {
        if (errorMessage?.Contains("no encontrado") == true)
          return NotFound(new { message = errorMessage });

        return BadRequest(new { message = errorMessage });
      }

      var puestoDto = _mapper.Map<PuestoDto>(updatedPuesto);
      return Ok(puestoDto);
    }
  }
}