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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios(int page = 1, int pageSize = 10)
        {
            var usuarios = await _usuarioService.GetUsuariosAsync(page, pageSize);
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(usuariosDto);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(uint id)
        {
            var user = await _usuarioService.GetUsuarioAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(user);
            return Ok(usuarioDto);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUsuario(uint id, [FromBody] UsuarioActualizarDto usuarioActualizarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (updatedUsuario, errorMessage) = await _usuarioService.UpdateUsuarioAsync(id, usuarioActualizarDto);

            if (updatedUsuario == null)
            {
                if (errorMessage == "Usuario no encontrado." || errorMessage == "Usuario no encontrado (error de concurrencia).")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(updatedUsuario);
            return Ok(usuarioDto);
        }

        // POST: api/Usuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCrearDto usuarioCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = _mapper.Map<Usuario>(usuarioCrearDto);

            var (createdUsuario, errorMessage) = await _usuarioService.CreateUsuarioAsync(usuario);

            if (createdUsuario == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(createdUsuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuarioDto.Id }, usuarioDto);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsuario(uint id)
        {
            var deleted = await _usuarioService.DeleteUsuarioAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "No se pudo eliminar el usuario. Puede que no exista o tenga elementos asociados (historial, puestos)." });
            }
            return NoContent();
        }
    }
}