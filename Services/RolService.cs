// TurneroApi/Services/RolService.cs
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
    public class RolService : IRolService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public RolService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Rol?> GetRolAsync(uint id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol)
        {
            // --- Normalización de datos ---
            rol.Tipo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(rol.Tipo.Trim().Replace(" ", "").ToLowerInvariant());
            rol.Tipo = Regex.Replace(rol.Tipo, @"\s+", " ").Trim();

            // --- Validaciones de Negocio ---
            var existingRolByType = await _context.Roles.FirstOrDefaultAsync(r => r.Tipo == rol.Tipo);
            if (existingRolByType != null)
            {
                return (null, $"El tipo de rol '{rol.Tipo}' ya está en uso. Debe ser único.");
            }

            _context.Roles.Add(rol);
            try
            {
                await _context.SaveChangesAsync();
                return (rol, null);
            }
            catch (DbUpdateException ex)
            {
                // Manejo de posibles errores de DB
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'tipo'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"El tipo de rol '{rol.Tipo}' ya está en uso. (DB Error)");
                }
                return (null, "Error al guardar el rol. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(uint id, RolActualizarDto rolActualizarDto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return (null, "Rol no encontrado.");
            }

            // --- Aplicar actualizaciones y normalización ---
            if (!string.IsNullOrEmpty(rolActualizarDto.Tipo))
            {
                string cleanedTipo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(rolActualizarDto.Tipo.Trim().Replace(" ", "").ToLowerInvariant());
                cleanedTipo = Regex.Replace(cleanedTipo, @"\s+", " ").Trim();

                if (cleanedTipo != rol.Tipo)
                {
                    // Validar unicidad antes de asignar
                    var existingRolByType = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Tipo == cleanedTipo && r.Id != id);
                    if (existingRolByType != null)
                    {
                        return (null, $"El tipo de rol '{cleanedTipo}' ya está en uso por otro rol. Debe ser único.");
                    }
                    rol.Tipo = cleanedTipo;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return (rol, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Roles.AnyAsync(e => e.Id == id))
                {
                    return (null, "Rol no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Manejo de posibles errores
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'tipo'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"El tipo de rol '{rol.Tipo}' ya está en uso. (DB Error)");
                }
                return (null, "Error al actualizar el rol. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<bool> DeleteRolAsync(uint id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return false;
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}