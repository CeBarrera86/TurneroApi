using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Mostrador;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class MostradorService : IMostradorService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;

  public MostradorService(TurneroDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

    public async Task<List<MostradorDto>> GetMostradoresAsync()
    {
      return await _context.Mostradores
            .AsNoTracking()
            .Include(m => m.MostradorSectores)
              .ThenInclude(ms => ms.Sector)
            .OrderBy(m => m.Numero)
            .ProjectTo<MostradorDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

  public async Task<MostradorDto?> GetMostradorAsync(int id)
  {
    return await _context.Mostradores
        .AsNoTracking()
        .Where(m => m.Id == id)
        .ProjectTo<MostradorDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
  }

  public async Task<(Mostrador? mostrador, string? errorMessage)> CreateMostradorAsync(Mostrador mostrador)
  {
    mostrador.Ip = NormalizarVariables.NormalizeIp(mostrador.Ip) ?? string.Empty;
    mostrador.Tipo = NormalizarVariables.NormalizeTipoMostrador(mostrador.Tipo);

    var ipError = await MostradorValidator.ValidateIpAsync(_context, mostrador.Ip);
    if (ipError != null) return (null, ipError);

    var numeroIpError = await MostradorValidator.ValidateNumeroIpUniqueAsync(_context, mostrador.Numero, mostrador.Ip);
    if (numeroIpError != null) return (null, numeroIpError);

    _context.Mostradores.Add(mostrador);

    try
    {
      await _context.SaveChangesAsync();

      // No asociamos sector aquí. Eso se hace con MostradorSectorService.
      await _context.Entry(mostrador)
          .Collection(m => m.MostradorSectores)
          .Query()
          .Include(ms => ms.Sector)
          .LoadAsync();

      return (mostrador, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, mostrador));
    }
  }

  public async Task<(Mostrador? mostrador, string? errorMessage)> UpdateMostradorAsync(int id, MostradorActualizarDto dto)
  {
    var mostrador = await _context.Mostradores
        .Include(m => m.MostradorSectores)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (mostrador == null) return (null, "Mostrador no encontrado.");

    _mapper.Map(dto, mostrador);

    mostrador.Ip = NormalizarVariables.NormalizeIp(mostrador.Ip) ?? string.Empty;
    mostrador.Tipo = NormalizarVariables.NormalizeTipoMostrador(mostrador.Tipo);

    if (dto.Ip != null)
    {
      var ipError = await MostradorValidator.ValidateIpAsync(_context, mostrador.Ip, id);
      if (ipError != null) return (null, ipError);
    }

    if (dto.Numero.HasValue || dto.Ip != null)
    {
      var numeroIpError = await MostradorValidator.ValidateNumeroIpUniqueAsync(_context, mostrador.Numero, mostrador.Ip, id);
      if (numeroIpError != null) return (null, numeroIpError);
    }

    try
    {
      await _context.SaveChangesAsync();

      await _context.Entry(mostrador)
          .Collection(m => m.MostradorSectores)
          .Query()
          .Include(ms => ms.Sector)
          .LoadAsync();

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

  public async Task<bool> DeleteMostradorAsync(int id)
  {
    var mostrador = await _context.Mostradores.FindAsync(id);
    if (mostrador == null) return false;

    // Si no tenés cascade, eliminar asociaciones primero
    var relaciones = await _context.MostradorSectores.Where(ms => ms.MostradorId == id).ToListAsync();
    if (relaciones.Count > 0)
    {
      _context.MostradorSectores.RemoveRange(relaciones);
    }

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
      if (ex.InnerException.Message.Contains("'numero'", StringComparison.OrdinalIgnoreCase) &&
          ex.InnerException.Message.Contains("'ip'", StringComparison.OrdinalIgnoreCase))
        return $"Ya existe un mostrador con número {mostrador.Numero} e IP {mostrador.Ip}. (DB Error)";
    }
    return "Error al guardar el mostrador. Verificá que los datos sean válidos y únicos.";
  }
}