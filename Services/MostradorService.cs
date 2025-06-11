using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class MostradorService : IMostradorService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public MostradorService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Mostrador>> GetMostradoresAsync()
        {
            return await _context.Mostradores.Include(m => m.SectorNavigation).ToListAsync();
        }

        public async Task<Mostrador?> GetMostradorAsync(uint id)
        {
            return await _context.Mostradores.Include(m => m.SectorNavigation).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<(Mostrador? mostrador, string? errorMessage)> CreateMostradorAsync(Mostrador mostrador)
        {
            // --- Normalización de datos ---
            mostrador.Ip = mostrador.Ip.Trim();
            if (!string.IsNullOrEmpty(mostrador.Tipo))
            {
                mostrador.Tipo = mostrador.Tipo.Trim().Replace(" ", "").ToUpperInvariant();
            }

            // --- Validaciones de Negocio ---
            var existingMostradorWithSameIp = await _context.Mostradores.FirstOrDefaultAsync(m => m.Ip == mostrador.Ip);
            if (existingMostradorWithSameIp != null)
            {
                return (null, $"La dirección IP '{mostrador.Ip}' ya está en uso. Debe ser única.");
            }

            var sectorExiste = await _context.Sectores.AnyAsync(s => s.Id == mostrador.SectorId);
            if (!sectorExiste)
            {
                return (null, $"El SectorId '{mostrador.SectorId}' proporcionado no es válido.");
            }

            var existingMostradorWithSameSectorAndNumber = await _context.Mostradores
                .FirstOrDefaultAsync(m => m.SectorId == mostrador.SectorId && m.Numero == mostrador.Numero);
            if (existingMostradorWithSameSectorAndNumber != null)
            {
                return (null, $"Ya existe un mostrador con el número '{mostrador.Numero}' en el SectorId '{mostrador.SectorId}'. La combinación de Sector y Número debe ser única.");
            }

            _context.Mostradores.Add(mostrador);
            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(mostrador).Reference(m => m.SectorNavigation).LoadAsync();
                return (mostrador, null);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (ex.InnerException.Message.Contains("'ip'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, "La dirección IP proporcionada ya está en uso. (DB Error)");
                    }
                    if (ex.InnerException.Message.Contains("'sector_id_numero_unique'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"Ya existe un mostrador con el número '{mostrador.Numero}' en el SectorId '{mostrador.SectorId}'. (DB Error)");
                    }
                }
                return (null, "Error al guardar el mostrador. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(uint id, MostradorActualizarDto mostradorActualizarDto)
        {
            var mostrador = await _context.Mostradores.FindAsync(id);
            if (mostrador == null)
            {
                return (null, "Mostrador no encontrado.");
            }

            // --- Actualizaciones y normalización ---
            _mapper.Map(mostradorActualizarDto, mostrador);

            if (!string.IsNullOrEmpty(mostrador.Ip))
            {
                 mostrador.Ip = mostrador.Ip.Trim();
            }
            if (!string.IsNullOrEmpty(mostrador.Tipo))
            {
                 mostrador.Tipo = mostrador.Tipo.Trim().Replace(" ", "").ToUpperInvariant();
            }

            // --- Validaciones de Negocio ---
            if (!string.IsNullOrEmpty(mostradorActualizarDto.Ip))
            {
                var existingMostradorWithSameIp = await _context.Mostradores
                    .FirstOrDefaultAsync(m => m.Ip == mostrador.Ip && m.Id != id);
                if (existingMostradorWithSameIp != null)
                {
                    return (null, $"La dirección IP '{mostrador.Ip}' ya está en uso por otro mostrador. Debe ser única.");
                }
            }

            if (mostradorActualizarDto.SectorId.HasValue)
            {
                var sectorExiste = await _context.Sectores.AnyAsync(s => s.Id == mostradorActualizarDto.SectorId.Value);
                if (!sectorExiste)
                {
                    return (null, $"El SectorId '{mostradorActualizarDto.SectorId.Value}' proporcionado no es válido.");
                }
            }

            var currentNumero = mostradorActualizarDto.Numero ?? mostrador.Numero;
            var currentSectorId = mostradorActualizarDto.SectorId ?? mostrador.SectorId;

            if (mostradorActualizarDto.Numero.HasValue || mostradorActualizarDto.SectorId.HasValue)
            {
                 var existingMostradorWithNewSectorAndNumber = await _context.Mostradores
                    .FirstOrDefaultAsync(m => m.SectorId == currentSectorId && m.Numero == currentNumero && m.Id != id);

                 if (existingMostradorWithNewSectorAndNumber != null)
                 {
                     return (null, $"Ya existe un mostrador con el número '{currentNumero}' en el SectorId '{currentSectorId}'. La combinación de Sector y Número debe ser única.");
                 }
            }

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(mostrador).Reference(m => m.SectorNavigation).LoadAsync();
                return (mostrador, null); // Éxito
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Mostradores.AnyAsync(e => e.Id == id))
                {
                    return (null, "Mostrador no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (ex.InnerException.Message.Contains("'ip'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, "La dirección IP proporcionada ya está en uso. (DB Error)");
                    }
                    if (ex.InnerException.Message.Contains("'sector_id_numero_unique'", StringComparison.OrdinalIgnoreCase))
                    {
                        return (null, $"Ya existe un mostrador con el número '{mostrador.Numero}' en el SectorId '{mostrador.SectorId}'. (DB Error)");
                    }
                }
                return (null, "Error al actualizar el mostrador. Asegúrate de que los datos sean únicos y válidos.");
            }
        }

        public async Task<bool> DeleteMostradorAsync(uint id)
        {
            var mostrador = await _context.Mostradores.FindAsync(id);
            if (mostrador == null)
            {
                return false;
            }

            _context.Mostradores.Remove(mostrador);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}