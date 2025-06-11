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
    public class EstadoService : IEstadoService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public EstadoService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Estado>> GetEstadosAsync()
        {
            return await _context.Estados.ToListAsync();
        }

        public async Task<Estado?> GetEstadoAsync(uint id)
        {
            return await _context.Estados.FindAsync(id);
        }

        public async Task<(Estado? estado, string? errorMessage)> CreateEstadoAsync(Estado estado)
        {
            // --- Normalización de datos ---
            estado.Letra = estado.Letra.Trim().ToUpperInvariant();
            estado.Descripcion = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(estado.Descripcion.Trim().ToLowerInvariant());
            estado.Descripcion = Regex.Replace(estado.Descripcion, @"\s+", " ").Trim();

            // --- Validaciones de Negocio ---
            var existingEstadoByLetra = await _context.Estados.FirstOrDefaultAsync(e => e.Letra == estado.Letra);
            if (existingEstadoByLetra != null)
            {
                return (null, $"Ya existe un estado con la letra '{estado.Letra}'. La letra debe ser única.");
            }

            _context.Estados.Add(estado);
            try
            {
                await _context.SaveChangesAsync();
                return (estado, null); // Éxito
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"La letra '{estado.Letra}' ya está en uso. (DB Error)");
                }
                return (null, "Error al guardar el estado. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<(Estado? estado, string? errorMessage)> UpdateEstadoAsync(uint id, EstadoActualizarDto estadoActualizarDto)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return (null, "Estado no encontrado.");
            }

            // --- Aplicar actualizaciones y normalización ---
            if (!string.IsNullOrEmpty(estadoActualizarDto.Letra))
            {
                string cleanedLetra = estadoActualizarDto.Letra.Trim().ToUpperInvariant();
                if (cleanedLetra != estado.Letra)
                {
                    // Validar unicidad antes de asignar
                    var existingEstadoByLetra = await _context.Estados
                        .FirstOrDefaultAsync(e => e.Letra == cleanedLetra && e.Id != id);
                    if (existingEstadoByLetra != null)
                    {
                        return (null, $"Ya existe un estado con la letra '{cleanedLetra}'. La letra debe ser única.");
                    }
                    estado.Letra = cleanedLetra;
                }
            }

            if (!string.IsNullOrEmpty(estadoActualizarDto.Descripcion))
            {
                string cleanedDescripcion = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(estadoActualizarDto.Descripcion.Trim().ToLowerInvariant());
                cleanedDescripcion = Regex.Replace(cleanedDescripcion, @"\s+", " ").Trim();
                if (cleanedDescripcion != estado.Descripcion)
                {
                    estado.Descripcion = cleanedDescripcion;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return (estado, null); // Éxito
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Estados.AnyAsync(e => e.Id == id))
                {
                    return (null, "Estado no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
                    ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
                {
                    return (null, $"La letra '{estado.Letra}' ya está en uso. (DB Error)");
                }
                return (null, "Error al actualizar el estado. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<bool> DeleteEstadoAsync(uint id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return false;
            }

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}