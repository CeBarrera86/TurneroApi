using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync(int page, int pageSize)
        {
            return await _context.Usuarios.Include(u => u.RolNavigation).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioAsync(uint id)
        {
            return await _context.Usuarios.Include(u => u.RolNavigation).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.Nombre))
            {
                usuario.Nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuario.Nombre.Trim().ToLowerInvariant());
                usuario.Nombre = Regex.Replace(usuario.Nombre, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(usuario.Apellido))
            {
                usuario.Apellido = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuario.Apellido.Trim().ToLowerInvariant());
                usuario.Apellido = Regex.Replace(usuario.Apellido, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(usuario.Username))
            {
                usuario.Username = usuario.Username.Trim().ToLowerInvariant();
                usuario.Username = Regex.Replace(usuario.Username, @"\s+", "");
            }

            // --- Validaciones de Negocio ---
            var existingUserByUsername = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == usuario.Username);
            if (existingUserByUsername != null)
            {
                return (null, $"El nombre de usuario '{usuario.Username}' ya está en uso. Debe ser único.");
            }

            // 2. Validación de RolId
            if (usuario.RolId == 0)
            {
                 return (null, "El RolId proporcionado no es válido (no puede ser 0).");
            }
            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == usuario.RolId);
            if (!rolExiste)
            {
                return (null, $"El RolId '{usuario.RolId}' proporcionado no es válido.");
            }

            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
                return (usuario, null);
            }
            catch (DbUpdateException ex)
            {
                // Manejo de posibles errores de DB
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'username'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"El nombre de usuario '{usuario.Username}' ya está en uso. (DB Error)");
                }
                return (null, "Error al guardar el usuario. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(uint id, UsuarioActualizarDto usuarioActualizarDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return (null, "Usuario no encontrado.");
            }

            // --- Aplicar actualizaciones y normalización ---
            // Manejo de Nombre
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Nombre))
            {
                string cleanedNombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioActualizarDto.Nombre.Trim().ToLowerInvariant());
                cleanedNombre = Regex.Replace(cleanedNombre, @"\s+", " ").Trim();
                usuario.Nombre = cleanedNombre;
            }

            // Manejo de Apellido
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Apellido))
            {
                string cleanedApellido = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(usuarioActualizarDto.Apellido.Trim().ToLowerInvariant());
                cleanedApellido = Regex.Replace(cleanedApellido, @"\s+", " ").Trim();
                usuario.Apellido = cleanedApellido;
            }

            // Manejo de Username
            if (!string.IsNullOrEmpty(usuarioActualizarDto.Username))
            {
                string cleanedUsername = usuarioActualizarDto.Username.Trim().ToLowerInvariant();
                cleanedUsername = Regex.Replace(cleanedUsername, @"\s+", "");

                if (cleanedUsername != usuario.Username)
                {
                    var existingUserWithSameUsername = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Username == cleanedUsername && u.Id != id);
                    if (existingUserWithSameUsername != null)
                    {
                        return (null, $"El nombre de usuario '{cleanedUsername}' ya está en uso por otro usuario. Debe ser único.");
                    }
                    usuario.Username = cleanedUsername;
                }
            }

            if (usuarioActualizarDto.RolId != 0 && usuarioActualizarDto.RolId != usuario.RolId)
            {
                var rolExiste = await _context.Roles.AnyAsync(r => r.Id == usuarioActualizarDto.RolId);
                if (!rolExiste)
                {
                    return (null, $"El RolId '{usuarioActualizarDto.RolId}' proporcionado no es válido.");
                }
                usuario.RolId = usuarioActualizarDto.RolId;
            }

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
                return (usuario, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Usuarios.AnyAsync(e => e.Id == id))
                {
                    return (null, "Usuario no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'username'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"El nombre de usuario ya está en uso. (DB Error)");
                }
                return (null, "Error al actualizar el usuario. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<bool> DeleteUsuarioAsync(uint id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            if (await _context.Historiales.AnyAsync(h => h.UsuarioId == id))
            {
                return false;
            }

            if (await _context.Puestos.AnyAsync(p => p.UsuarioId == id))
            {
                return false;
            }


            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}