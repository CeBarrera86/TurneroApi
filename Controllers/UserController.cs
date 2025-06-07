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
    public class UserController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public UserController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.RoleNavigation)  // <-- Esto carga la relación necesaria
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(ulong id)
        {
            var user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(ulong id, UserUpdateDto UserUpdateDto)
        {
            var user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            _mapper.Map(UserUpdateDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                throw;
            }

            await _context.Entry(user).Reference(u => u.RoleNavigation).LoadAsync();
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> PostUser(UserCreateDto userCreateDto)
        {
            // Validaciones básicas (puedes usar FluentValidation o ModelState)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO a entidad User
            var user = _mapper.Map<User>(userCreateDto);

            // Hash de contraseña (ejemplo simplificado)
            if (!string.IsNullOrEmpty(userCreateDto.Password))
            {
                var hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, userCreateDto.Password);
            }
            else
            {
                // Maneja el caso de password vacío, por ejemplo error o default
                return BadRequest("Password is required.");
            }

            // Campos automáticos
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).Reference(u => u.RoleNavigation).LoadAsync();
            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(ulong id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(ulong id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
