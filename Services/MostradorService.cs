using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

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
      mostrador.Ip = NormalizarVariables.NormalizeIp(mostrador.Ip) ?? string.Empty;
      mostrador.Tipo = NormalizarVariables.NormalizeTipoMostrador(mostrador.Tipo);

      var ipError = await MostradorValidator.ValidateIpAsync(_context, mostrador.Ip);
      if (ipError != null) return (null, ipError);

      var sectorError = await MostradorValidator.ValidateSectorIdAsync(_context, mostrador.SectorId);
      if (sectorError != null) return (null, sectorError);

      var numeroError = await MostradorValidator.ValidateNumeroUnicoAsync(_context, mostrador.SectorId, mostrador.Numero);
      if (numeroError != null) return (null, numeroError);

      _context.Mostradores.Add(mostrador);
      try
      {
        await _context.SaveChangesAsync();
        await _context.Entry(mostrador).Reference(m => m.SectorNavigation).LoadAsync();
        return (mostrador, null);
      }
      catch (DbUpdateException ex)
      {
        return (null, InterpretarDbError(ex, mostrador));
      }
    }

    public async Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(uint id, MostradorActualizarDto dto)
    {
      var mostrador = await _context.Mostradores.FindAsync(id);
      if (mostrador == null) return (null, "Mostrador no encontrado.");

      _mapper.Map(dto, mostrador);

      mostrador.Ip = NormalizarVariables.NormalizeIp(mostrador.Ip) ?? string.Empty;
      mostrador.Tipo = NormalizarVariables.NormalizeTipoMostrador(mostrador.Tipo);

      if (!string.IsNullOrEmpty(dto.Ip))
      {
        var ipError = await MostradorValidator.ValidateIpAsync(_context, mostrador.Ip, id);
        if (ipError != null) return (null, ipError);
      }

      if (dto.SectorId.HasValue)
      {
        var sectorError = await MostradorValidator.ValidateSectorIdAsync(_context, dto.SectorId.Value);
        if (sectorError != null) return (null, sectorError);
      }

      var currentNumero = dto.Numero ?? mostrador.Numero;
      var currentSectorId = dto.SectorId ?? mostrador.SectorId;

      if (dto.Numero.HasValue || dto.SectorId.HasValue)
      {
        var numeroError = await MostradorValidator.ValidateNumeroUnicoAsync(_context, currentSectorId, currentNumero, id);
        if (numeroError != null) return (null, numeroError);
      }

      try
      {
        await _context.SaveChangesAsync();
        await _context.Entry(mostrador).Reference(m => m.SectorNavigation).LoadAsync();
        return (mostrador, null);
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!await _context.Mostradores.AnyAsync(e => e.Id == id))
          return (null, "Mostrador no encontrado (error de concurrencia).");
        throw;
      }
      catch (DbUpdateException ex)
      {
        return (null, InterpretarDbError(ex, mostrador));
      }
    }

    public async Task<bool> DeleteMostradorAsync(uint id)
    {
      var mostrador = await _context.Mostradores.FindAsync(id);
      if (mostrador == null) return false;

      _context.Mostradores.Remove(mostrador);
      await _context.SaveChangesAsync();
      return true;
    }

    private string InterpretarDbError(DbUpdateException ex, Mostrador mostrador)
    {
      if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
      {
        if (ex.InnerException.Message.Contains("'ip'", StringComparison.OrdinalIgnoreCase))
          return $"La IP '{mostrador.Ip}' ya está en uso. (DB Error)";
        if (ex.InnerException.Message.Contains("'sector_id'", StringComparison.OrdinalIgnoreCase) &&
            ex.InnerException.Message.Contains("'numero'", StringComparison.OrdinalIgnoreCase))
          return $"Ya existe un mostrador número {mostrador.Numero} en el sector {mostrador.SectorId}. (DB Error)";
      }
      return "Error al guardar el mostrador. Verificá que los datos sean válidos y únicos.";
    }
  }
}