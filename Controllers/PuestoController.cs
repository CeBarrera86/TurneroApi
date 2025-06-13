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
    public class PuestoController : ControllerBase
    {
        private readonly IPuestoService _puestoService;
        private readonly IMapper _mapper;

        public PuestoController(IPuestoService puestoService, IMapper mapper)
        {
            _puestoService = puestoService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PuestoDto>> GetPuesto(uint id)
        {
            var puesto = await _puestoService.GetPuestoAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }
            var puestoDto = _mapper.Map<PuestoDto>(puesto);
            return Ok(puestoDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Usuario")]
        public async Task<ActionResult<PuestoDto>> PostPuesto([FromBody] PuestoCrearDto puestoCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (createdPuesto, errorMessage) = await _puestoService.CreatePuestoAsync(puestoCrearDto);

            if (createdPuesto == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var puestoDto = _mapper.Map<PuestoDto>(createdPuesto);
            return CreatedAtAction(nameof(GetPuesto), new { id = puestoDto.Id }, puestoDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Usuario")]
        public async Task<IActionResult> PutPuesto(uint id, [FromBody] PuestoActualizarDto puestoActualizarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (updatedPuesto, errorMessage) = await _puestoService.UpdatePuestoAsync(id, puestoActualizarDto);

            if (updatedPuesto == null)
            {
                if (errorMessage == "Puesto no encontrado." || errorMessage == "Puesto no encontrado (error de concurrencia).")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            var puestoDto = _mapper.Map<PuestoDto>(updatedPuesto);
            return Ok(puestoDto);
        }

        // POST: api/Puesto/{id}/login (Endpoint específico para registrar el login de un usuario en un puesto)
        [HttpPost("{id}/login")]
        // [Authorize] // Cualquier usuario logueado podría iniciar sesión en un puesto
        public async Task<ActionResult<PuestoDto>> LoginPuesto(uint id, [FromBody] PuestoLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Aquí podríamos obtener el UsuarioId del token JWT si fuera necesario,
            // pero para flexibilidad lo recibimos en el DTO por ahora.
            var (loggedInPuesto, errorMessage) = await _puestoService.RegistrarLoginAsync(id, loginDto.UsuarioId);

            if (loggedInPuesto == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var puestoDto = _mapper.Map<PuestoDto>(loggedInPuesto);
            return Ok(puestoDto);
        }

        // POST: api/Puesto/{id}/logout (Endpoint específico para registrar el logout de un usuario de un puesto)
        [HttpPost("{id}/logout")]
        // [Authorize] // Cualquier usuario logueado podría cerrar sesión en su puesto
        public async Task<ActionResult<PuestoDto>> LogoutPuesto(uint id)
        {
            var (loggedOutPuesto, errorMessage) = await _puestoService.RegistrarLogoutAsync(id);

            if (loggedOutPuesto == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var puestoDto = _mapper.Map<PuestoDto>(loggedOutPuesto);
            return Ok(puestoDto);
        }
    }
}