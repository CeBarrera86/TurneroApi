using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs.Usuario;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers;

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
	[Authorize(Policy = "ver_usuario")]
	public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios(int page = 1, int pageSize = 10)
	{
		var usuarios = await _usuarioService.GetUsuariosAsync();
		var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
		return Ok(usuariosDto);
	}

	// GET: api/Usuario/5
	[HttpGet("{id}")]
	[Authorize(Policy = "ver_usuario")]
	public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
	{
		var user = await _usuarioService.GetUsuarioAsync(id);
		if (user == null) return NotFound();

		var usuarioDto = _mapper.Map<UsuarioDto>(user);
		return Ok(usuarioDto);
	}

	// POST: api/Usuario
	[HttpPost]
	[Authorize(Policy = "crear_usuario")]
	public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCrearDto usuarioCrearDto)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var usuario = _mapper.Map<Usuario>(usuarioCrearDto);
		var (createdUsuario, errorMessage) = await _usuarioService.CreateUsuarioAsync(usuario);

		if (createdUsuario == null) return BadRequest(new { message = errorMessage });

		var usuarioDto = _mapper.Map<UsuarioDto>(createdUsuario);
		return CreatedAtAction(nameof(GetUsuario), new { id = usuarioDto.Id }, usuarioDto);
	}

	// PUT: api/Usuario/5
	[HttpPut("{id}")]
	[Authorize(Policy = "editar_usuario")]
	public async Task<ActionResult<UsuarioDto>> PutUsuario(int id, [FromBody] UsuarioActualizarDto usuarioActualizarDto)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var (updatedUsuario, errorMessage) = await _usuarioService.UpdateUsuarioAsync(id, usuarioActualizarDto);
		if (updatedUsuario == null)
		{
			if (errorMessage?.Contains("no encontrado") == true)
				return NotFound(new { message = errorMessage });
			return BadRequest(new { message = errorMessage });
		}

		var usuarioDto = _mapper.Map<UsuarioDto>(updatedUsuario);
		return Ok(usuarioDto);
	}

	// DELETE: api/Usuario/5
	[HttpDelete("{id}")]
	[Authorize(Policy = "eliminar_usuario")]
	public async Task<IActionResult> DeleteUsuario(int id)
	{
		var deleted = await _usuarioService.DeleteUsuarioAsync(id);
		if (!deleted)
		{
			return NotFound(new { message = "No se pudo eliminar el usuario. Puede que no exista o tenga elementos asociados (historial, puestos)." });
		}
		return NoContent();
	}
}