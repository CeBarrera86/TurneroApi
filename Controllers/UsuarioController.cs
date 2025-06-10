using AutoMapper;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Models;

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
        public async Task<IActionResult> PutUsuario(uint id, [FromBody] UsuarioActualizarDto usuarioActualizarDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                return NotFound();

            // --- Manejo y Normalización de Nombre ---
            string? nombre = null;
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Nombre))
            {
                nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioActualizarDto.Nombre.Trim().ToLowerInvariant());
                nombre = Regex.Replace(nombre, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(nombre) && nombre != usuario.Nombre)
            {
                usuario.Nombre = nombre;
            }

            // --- Manejo y Normalización de Apellido ---
            string? apellido = null;
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Apellido))
            {
                apellido = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioActualizarDto.Apellido.Trim().ToLowerInvariant());
                apellido = Regex.Replace(apellido, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(apellido) && apellido != usuario.Apellido)
            {
                usuario.Apellido = apellido;
            }

            // --- Manejo y Normalización de Username ---
            string? username = null;
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Username))
            {
                username = usuarioActualizarDto.Username.Trim().ToLowerInvariant();
                username = Regex.Replace(username, @"\s+", "");
            }

            if (!string.IsNullOrEmpty(username) && username != usuario.Username)
            {
                var existingUserWithSameUsername = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == username && u.Id != id);

                if (existingUserWithSameUsername != null)
                {
                    return BadRequest(new { message = $"El nombre de usuario '{username}' ya está en uso por otro usuario. Debe ser único." });
                }
                usuario.Username = username;
            }

            // --- Manejo de RolId ---
            if (usuarioActualizarDto.RolId != 0 && usuarioActualizarDto.RolId != usuario.RolId)
            {
                var rolExiste = await _context.Roles.AnyAsync(r => r.Id == usuarioActualizarDto.RolId);
                if (!rolExiste)
                {
                    return BadRequest(new { message = $"El RolId '{usuarioActualizarDto.RolId}' proporcionado no es válido." });
                }
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
            catch (DbUpdateException) 
            {
                return StatusCode(500, new { message = "Error al actualizar el usuario. Asegúrate de que el nombre de usuario sea único." });
            }

            await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return Ok(usuarioDto);
        }

        // POST: api/Usuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCrearDto usuarioCrearDto)
        {
            // --- Limpieza y Normalización de Datos de Entrada ---
            string? nombre = null;
            string? apellido = null;
            string? username = null;

            if (!string.IsNullOrEmpty(usuarioCrearDto.Nombre))
            {
                nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioCrearDto.Nombre.Trim().ToLowerInvariant());
                nombre = Regex.Replace(nombre, @"\s+", " ").Trim();
            }
            
            if (!string.IsNullOrEmpty(usuarioCrearDto.Apellido))
            {
                apellido = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioCrearDto.Apellido.Trim().ToLowerInvariant());
                apellido = Regex.Replace(apellido, @"\s+", " ").Trim();
            }
            
            if (!string.IsNullOrEmpty(usuarioCrearDto.Username))
            {
                username = usuarioCrearDto.Username.Trim().ToLowerInvariant();
                username = Regex.Replace(username, @"\s+", "");
            }

            // Validación de campos obligatorios
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest(new { message = "El nombre del usuario no puede estar vacío." });
            }
            if (string.IsNullOrEmpty(apellido))
            {
                return BadRequest(new { message = "El apellido del usuario no puede estar vacío." });
            }
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { message = "El nombre de usuario no puede estar vacío." });
            }

            // Validación de unicidad para Username
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
            if (usuarioExistente != null)
            {
                return BadRequest(new { message = $"El nombre de usuario '{username}' ya está en uso. Debe ser único." });
            }

            // Validación de RolId
            if (usuarioCrearDto.RolId == 0)
            {
                return BadRequest(new { message = "El RolId proporcionado no es válido (no puede ser 0)." });
            }
            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == usuarioCrearDto.RolId);
            if (!rolExiste)
            {
                return BadRequest(new { message = $"El RolId '{usuarioCrearDto.RolId}' proporcionado no es válido." });
            }

            var usuario = _mapper.Map<Usuario>(usuarioCrearDto);

            usuario.Nombre = nombre!;
            usuario.Apellido = apellido!;
            usuario.Username = username!;
            usuario.RolId = usuarioCrearDto.RolId;

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error al guardar el usuario. Asegúrate de que el nombre de usuario sea único y que no haya otros problemas de la base de datos." });
            }

            await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuarioDto.Id }, usuarioDto);
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
