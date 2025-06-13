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
    [Authorize]
    public class MostradorController : ControllerBase
    {
        private readonly IMostradorService _mostradorService;
        private readonly IMapper _mapper;

        public MostradorController(IMostradorService mostradorService, IMapper mapper)
        {
            _mostradorService = mostradorService;
            _mapper = mapper;
        }

        // GET: api/Mostrador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MostradorDto>>> GetMostradores()
        {
            var mostradores = await _mostradorService.GetMostradoresAsync();
            var mostradoresDto = _mapper.Map<IEnumerable<MostradorDto>>(mostradores);
            return Ok(mostradoresDto);
        }

        // GET: api/Mostrador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MostradorDto>> GetMostrador(uint id)
        {
            var mostrador = await _mostradorService.GetMostradorAsync(id);
            if (mostrador == null)
            {
                return NotFound();
            }

            var mostradorDto = _mapper.Map<MostradorDto>(mostrador);
            return Ok(mostradorDto);
        }

        // PUT: api/Mostrador/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMostrador(uint id, [FromBody] MostradorActualizarDto mostradorActualizarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (updatedMostrador, errorMessage) = await _mostradorService.UpdateMostradorAsync(id, mostradorActualizarDto);

            if (updatedMostrador == null)
            {
                if (errorMessage == "Mostrador no encontrado." || errorMessage == "Mostrador no encontrado (error de concurrencia).")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            var mostradorDto = _mapper.Map<MostradorDto>(updatedMostrador);
            return Ok(mostradorDto);
        }

        // POST: api/Mostrador
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MostradorDto>> PostMostrador([FromBody] MostradorCrearDto mostradorCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mostrador = _mapper.Map<Mostrador>(mostradorCrearDto);

            var (createdMostrador, errorMessage) = await _mostradorService.CreateMostradorAsync(mostrador);

            if (createdMostrador == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var mostradorDto = _mapper.Map<MostradorDto>(createdMostrador);

            return CreatedAtAction(nameof(GetMostrador), new { id = mostradorDto.Id }, mostradorDto);
        }

        // DELETE: api/Mostrador/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMostrador(uint id)
        {
            var deleted = await _mostradorService.DeleteMostradorAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}