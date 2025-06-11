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
    public class SectorService : ISectorService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public SectorService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Sector>> GetSectoresAsync()
        {
            return await _context.Sectores.Include(s => s.Padre).ToListAsync();
        }

        public async Task<Sector?> GetSectorAsync(uint id)
        {
            return await _context.Sectores.Include(s => s.Padre).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<(Sector? sector, string? errorMessage)> CreateSectorAsync(Sector sector)
        {
            // --- Normalización de datos ---
            if (!string.IsNullOrEmpty(sector.Letra))
            {
                sector.Letra = sector.Letra.Trim().Replace(" ", "").ToUpperInvariant();
            }
            if (!string.IsNullOrEmpty(sector.Nombre))
            {
                sector.Nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sector.Nombre.Trim().ToLowerInvariant());
                sector.Nombre = Regex.Replace(sector.Nombre, @"\s+", " ").Trim();
            }
            if (!string.IsNullOrEmpty(sector.Descripcion))
            {
                sector.Descripcion = sector.Descripcion.Trim().Replace(" ", "").ToUpperInvariant();
            }

            // --- Validaciones de Negocio ---
            // 1. Unicidad de Letra
            if (!string.IsNullOrEmpty(sector.Letra))
            {
                var existingSectorByLetra = await _context.Sectores.FirstOrDefaultAsync(s => s.Letra == sector.Letra);
                if (existingSectorByLetra != null)
                {
                    return (null, $"La letra '{sector.Letra}' ya está en uso. Debe ser única.");
                }
            }
            else
            {
            }

            // 2. Unicidad de Nombre
            if (!string.IsNullOrEmpty(sector.Nombre))
            {
                var existingSectorByName = await _context.Sectores.FirstOrDefaultAsync(s => s.Nombre == sector.Nombre);
                if (existingSectorByName != null)
                {
                    return (null, $"El nombre '{sector.Nombre}' ya está en uso. Debe ser único.");
                }
            }
            else
            {
                // Similar a Letra.
            }

            // 3. Validación de PadreId
            if (sector.PadreId.HasValue)
            {
                var padreExiste = await _context.Sectores.AnyAsync(s => s.Id == sector.PadreId.Value);
                if (!padreExiste)
                {
                    return (null, $"El PadreId '{sector.PadreId.Value}' proporcionado no es válido.");
                }
            }

            _context.Sectores.Add(sector);
            try
            {
                await _context.SaveChangesAsync();
                // Cargamos la propiedad Padre después de guardar para que el DTO la tenga
                if (sector.PadreId.HasValue)
                {
                    await _context.Entry(sector).Reference(s => s.Padre).LoadAsync();
                }
                return (sector, null); // Éxito
            }
            catch (DbUpdateException ex)
            {
                // Manejo de posibles errores de DB
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"La letra '{sector.Letra}' ya está en uso. (DB Error)");
                    }
                    if (ex.InnerException.Message.Contains("'nombre'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"El nombre '{sector.Nombre}' ya está en uso. (DB Error)");
                    }
                }
                return (null, "Error al guardar el sector. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(uint id, SectorActualizarDto sectorActualizarDto)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
            {
                return (null, "Sector no encontrado.");
            }

            // --- Aplicar actualizaciones y normalización ---
            // Manejo de Letra
            if (!string.IsNullOrEmpty(sectorActualizarDto.Letra))
            {
                string cleanedLetra = sectorActualizarDto.Letra.Trim().Replace(" ", "").ToUpperInvariant();
                if (cleanedLetra != sector.Letra)
                {
                    var existingSectorByLetra = await _context.Sectores
                        .FirstOrDefaultAsync(s => s.Letra == cleanedLetra && s.Id != id);
                    if (existingSectorByLetra != null)
                    {
                        return (null, $"La letra '{cleanedLetra}' ya está en uso por otro sector. Debe ser única.");
                    }
                    sector.Letra = cleanedLetra;
                }
            }
            else if (sectorActualizarDto.Letra == null)
            {
                sector.Letra = null;
            }

            // Manejo de Nombre
            if (!string.IsNullOrEmpty(sectorActualizarDto.Nombre))
            {
                string cleanedNombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sectorActualizarDto.Nombre.Trim().ToLowerInvariant());
                cleanedNombre = Regex.Replace(cleanedNombre, @"\s+", " ").Trim();

                if (cleanedNombre != sector.Nombre)
                {
                    var existingSectorByName = await _context.Sectores
                        .FirstOrDefaultAsync(s => s.Nombre == cleanedNombre && s.Id != id);
                    if (existingSectorByName != null)
                    {
                        return (null, $"El nombre '{cleanedNombre}' ya está en uso por otro sector. Debe ser único.");
                    }
                    sector.Nombre = cleanedNombre;
                }
            }
            else if (sectorActualizarDto.Nombre == null)
            {
                sector.Nombre = null;
            }

            // Manejo de Descripción
            if (!string.IsNullOrEmpty(sectorActualizarDto.Descripcion))
            {
                string cleanedDescripcion = sectorActualizarDto.Descripcion.Trim().Replace(" ", "").ToUpperInvariant();
                if (cleanedDescripcion != sector.Descripcion)
                {
                    sector.Descripcion = cleanedDescripcion;
                }
            }
            else if (sectorActualizarDto.Descripcion == null)
            {
                sector.Descripcion = null;
            }

            // Manejo de PadreId
            if (sectorActualizarDto.PadreId.HasValue)
            {
                // Un sector no puede ser su propio padre
                if (sectorActualizarDto.PadreId.Value == id)
                {
                    return (null, "Un sector no puede ser su propio padre.");
                }

                // Validar que el PadreId exista
                var padreExiste = await _context.Sectores.AnyAsync(s => s.Id == sectorActualizarDto.PadreId.Value);
                if (!padreExiste)
                {
                    return (null, $"El PadreId '{sectorActualizarDto.PadreId.Value}' proporcionado no es válido.");
                }

                // Actualizar si el PadreId es diferente
                if (sectorActualizarDto.PadreId.Value != sector.PadreId)
                {
                    sector.PadreId = sectorActualizarDto.PadreId.Value;
                }
            }
            else if (sectorActualizarDto.PadreId == null && sector.PadreId.HasValue)
            {
                sector.PadreId = null;
            }


            try
            {
                await _context.SaveChangesAsync();
                // Cargamos la propiedad Padre después de guardar para que el DTO la tenga
                if (sector.PadreId.HasValue)
                {
                    await _context.Entry(sector).Reference(s => s.Padre).LoadAsync();
                }
                return (sector, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Sectores.AnyAsync(e => e.Id == id))
                {
                    return (null, "Sector no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Manejo de posibles errores de DB
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"La letra '{sector.Letra}' ya está en uso. (DB Error)");
                    }
                    if (ex.InnerException.Message.Contains("'nombre'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"El nombre '{sector.Nombre}' ya está en uso. (DB Error)");
                    }
                }
                return (null, "Error al actualizar el sector. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<bool> DeleteSectorAsync(uint id)
        {
            var sector = await _context.Sectores.FindAsync(id);
            if (sector == null)
            {
                return false;
            }

            if (await _context.Mostradores.AnyAsync(m => m.SectorId == id))
            {
                return false;
            }

            if (await _context.Sectores.AnyAsync(s => s.PadreId == id))
            {
                return false;
            }

            _context.Sectores.Remove(sector);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}