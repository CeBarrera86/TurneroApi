using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services;

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
    sector.Letra = NormalizarVariables.NormalizeLetraSector(sector.Letra);
    sector.Nombre = NormalizarVariables.NormalizeNombre(sector.Nombre);
    sector.Descripcion = NormalizarVariables.NormalizeDescripcionSector(sector.Descripcion);

    var letraError = await SectorValidator.ValidateLetraAsync(_context, sector.Letra);
    if (letraError != null) return (null, letraError);

    var nombreError = await SectorValidator.ValidateNombreAsync(_context, sector.Nombre);
    if (nombreError != null) return (null, nombreError);

    if (sector.PadreId.HasValue)
    {
      var padreError = await SectorValidator.ValidatePadreIdAsync(_context, sector.PadreId, 0);
      if (padreError != null) return (null, padreError);
    }

    _context.Sectores.Add(sector);
    try
    {
      await _context.SaveChangesAsync();
      await CargarPadreAsync(sector);
      return (sector, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, sector));
    }
  }

  public async Task<(Sector? sector, string? errorMessage)> UpdateSectorAsync(uint id, SectorActualizarDto dto)
  {
    var sector = await _context.Sectores.FindAsync(id);
    if (sector == null) return (null, "Sector no encontrado.");

    var letra = NormalizarVariables.NormalizeLetraSector(dto.Letra);
    if (letra != sector.Letra)
    {
      var letraError = await SectorValidator.ValidateLetraAsync(_context, letra, id);
      if (letraError != null) return (null, letraError);
      sector.Letra = letra;
    }
    else if (dto.Letra == null)
    {
      sector.Letra = null;
    }

    var nombre = NormalizarVariables.NormalizeNombre(dto.Nombre);
    if (nombre != sector.Nombre)
    {
      var nombreError = await SectorValidator.ValidateNombreAsync(_context, nombre, id);
      if (nombreError != null) return (null, nombreError);
      sector.Nombre = nombre;
    }
    else if (dto.Nombre == null)
    {
      sector.Nombre = null;
    }

    var descripcion = NormalizarVariables.NormalizeDescripcionSector(dto.Descripcion);
    if (descripcion != sector.Descripcion)
    {
      sector.Descripcion = descripcion;
    }
    else if (dto.Descripcion == null)
    {
      sector.Descripcion = null;
    }

    if (dto.Activo.HasValue)
    {
      sector.Activo = dto.Activo.Value;
    }

    if (dto.PadreId.HasValue)
    {
      var padreError = await SectorValidator.ValidatePadreIdAsync(_context, dto.PadreId.Value, id);
      if (padreError != null) return (null, padreError);
      sector.PadreId = dto.PadreId.Value;
    }
    else if (dto.PadreId == null && sector.PadreId.HasValue)
    {
      sector.PadreId = null;
    }

    try
    {
      await _context.SaveChangesAsync();
      await CargarPadreAsync(sector);
      return (sector, null);
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _context.Sectores.AnyAsync(e => e.Id == id))
        return (null, "Sector no encontrado (error de concurrencia).");
      throw;
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, sector));
    }
  }

  public async Task<IEnumerable<Sector>> GetSectoresActivosAsync()
  {
    return await _context.Sectores.Include(s => s.Padre).Where(s => s.Activo).ToListAsync();
  }

  public async Task<IEnumerable<Sector>> GetSectoresActivosPadresAsync()
  {
    return await _context.Sectores.Where(s => s.Activo && s.PadreId == null).ToListAsync();
  }

  public async Task<(bool deleted, string? errorMessage)> DeleteSectorAsync(uint id)
  {
    var sector = await _context.Sectores.FindAsync(id);
    if (sector == null) return (false, "El sector no existe.");

    if (await _context.Mostradores.AnyAsync(m => m.SectorId == id))
      return (false, "El sector tiene mostradores asociados y no puede eliminarse.");

    if (await _context.Sectores.AnyAsync(s => s.PadreId == id))
      return (false, "El sector tiene sectores hijos y no puede eliminarse.");

    _context.Sectores.Remove(sector);
    await _context.SaveChangesAsync();
    return (true, null);
  }

  // --- Métodos privados reutilizables ---
  private async Task CargarPadreAsync(Sector sector)
  {
    if (sector.PadreId.HasValue)
    {
      await _context.Entry(sector).Reference(s => s.Padre).LoadAsync();
    }
  }

  private string InterpretarDbError(DbUpdateException ex, Sector sector)
  {
    if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
    {
      if (ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
        return $"La letra '{sector.Letra}' ya está en uso. (DB Error)";
      if (ex.InnerException.Message.Contains("'nombre'", StringComparison.OrdinalIgnoreCase))
        return $"El nombre '{sector.Nombre}' ya está en uso. (DB Error)";
    }
    return "Error al guardar el sector. Asegúrate de que los datos sean únicos y válidos.";
  }
}