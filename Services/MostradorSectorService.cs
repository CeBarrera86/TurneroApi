using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.MostradorSector;
using TurneroApi.Interfaces;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class MostradorSectorService : IMostradorSectorService
{
  private readonly TurneroDbContext _context;

  public MostradorSectorService(TurneroDbContext context)
  {
    _context = context;
  }

  public async Task<(bool ok, string? error)> AsociarAsync(int mostradorId, int sectorId)
  {
    var idError = await MostradorSectorValidator.ValidateIdsAsync(_context, mostradorId, sectorId);
    if (idError != null) return (false, idError);

    var existeError = await MostradorSectorValidator.ValidateExistenciaAsync(_context, mostradorId, sectorId);
    if (existeError != null) return (false, existeError);

    _context.MostradorSectores.Add(new Models.MostradorSector
    {
      MostradorId = mostradorId,
      SectorId = sectorId,
    });

    try
    {
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (DbUpdateException ex)
    {
      return (false, $"Error al asociar mostrador con sector: {ex.Message}");
    }
  }

  public async Task<(bool ok, string? error)> DesasociarAsync(int mostradorId, int sectorId)
  {
    var relacion = await _context.MostradorSectores.FindAsync(mostradorId, sectorId);
    if (relacion == null) return (false, "La asociación mostrador-sector no existe.");

    _context.MostradorSectores.Remove(relacion);
    try
    {
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (DbUpdateException ex)
    {
      return (false, $"Error al desasociar mostrador de sector: {ex.Message}");
    }
  }

  public async Task<IEnumerable<MostradorSectorItemDto>> GetSectoresPorMostradorAsync(int mostradorId)
  {
    var query = _context.MostradorSectores.Where(ms => ms.MostradorId == mostradorId).Include(ms => ms.Mostrador).Include(ms => ms.Sector)
        .Select(ms => new MostradorSectorItemDto
        {
          MostradorId = ms.MostradorId,
          SectorId = ms.SectorId,
          MostradorIp = ms.Mostrador.Ip,
          MostradorNumero = ms.Mostrador.Numero,
          SectorNombre = ms.Sector.Nombre,
          SectorLetra = ms.Sector.Letra
        })
        .AsNoTracking();

    return await query.ToListAsync();
  }

  public async Task<IEnumerable<MostradorSectorItemDto>> GetMostradoresPorSectorAsync(int sectorId)
  {
    var query = _context.MostradorSectores.Where(ms => ms.SectorId == sectorId).Include(ms => ms.Mostrador).Include(ms => ms.Sector)
        .Select(ms => new MostradorSectorItemDto
        {
          MostradorId = ms.MostradorId,
          SectorId = ms.SectorId,
          MostradorIp = ms.Mostrador.Ip,
          MostradorNumero = ms.Mostrador.Numero,
          SectorNombre = ms.Sector.Nombre,
          SectorLetra = ms.Sector.Letra
        })
        .AsNoTracking();

    return await query.ToListAsync();
  }
}