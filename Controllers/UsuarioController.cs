using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios(int page = 1, int pageSize = 10)
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.RolNavigation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(usuariosDto);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(uint id)
        {
            var user = await _context.Usuarios
                .Include(u => u.RolNavigation)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var usuarioDto = _mapper.Map<UsuarioDto>(user);
            return Ok(usuarioDto);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUsuario(uint id, UsuarioActualizarDto usuarioActualizarDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            // Nombre
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Nombre) && usuarioActualizarDto.Nombre != usuario.Nombre)
            {
                usuario.Nombre = usuarioActualizarDto.Nombre;
            }
            // Apellido
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Apellido) && usuarioActualizarDto.Apellido != usuario.Apellido)
            {
                usuario.Apellido = usuarioActualizarDto.Apellido;
            }
            // Username
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Username) && usuarioActualizarDto.Username != usuario.Username)
            {
                usuario.Username = usuarioActualizarDto.Username;
            }
            // RolId
            if (usuarioActualizarDto.RolId != 0 && usuarioActualizarDto.RolId != usuario.RolId)
            {
                usuario.RolId = usuarioActualizarDto.RolId;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                throw;
            }

            await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return Ok(usuarioDto);
        }

        // POST: api/Usuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioCrearDto>> PostUsuario(UsuarioCrearDto UsuarioCrearDto)
        {
            var usuario = _mapper.Map<Usuario>(UsuarioCrearDto);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioDto);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsuario(uint id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(uint id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
