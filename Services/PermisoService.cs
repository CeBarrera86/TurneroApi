using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Permiso;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Validation;
using TurneroApi.Utils;

namespace TurneroApi.Services;

public class PermisoService : IPermisoService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;

  public PermisoService(TurneroDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<PagedResult<PermisoDto>> GetPermisosAsync(int page, int pageSize)
  {
    var query = _context.Permisos
      .AsNoTracking()
      .OrderBy(p => p.Id)
      .ProjectTo<PermisoDto>(_mapper.ConfigurationProvider);

    return await query.ToPagedResultAsync(page, pageSize);
  }

  public async Task<PermisoDto?> GetPermisoAsync(int id)
  {
    return await _context.Permisos
      .AsNoTracking()
      .Where(p => p.Id == id)
      .ProjectTo<PermisoDto>(_mapper.ConfigurationProvider)
      .FirstOrDefaultAsync();
  }

  public async Task<(Permiso? permiso, string? errorMessage)> CreatePermisoAsync(Permiso permiso)
  {
    var nombreError = await PermisoValidator.ValidateNombreAsync(_context, permiso.Nombre);
    if (nombreError != null) return (null, nombreError);

    _context.Permisos.Add(permiso);
    try
    {
      await _context.SaveChangesAsync();
      return (permiso, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, permiso));
    }
  }

  public async Task<(Permiso? permiso, string? errorMessage)> UpdatePermisoAsync(int id, PermisoActualizarDto dto)
  {
    var permiso = await _context.Permisos.FindAsync(id);
    if (permiso == null) return (null, "Permiso no encontrado.");

    if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre != permiso.Nombre)
    {
      var nombreError = await PermisoValidator.ValidateNombreAsync(_context, dto.Nombre, id);
      if (nombreError != null) return (null, nombreError);
      permiso.Nombre = dto.Nombre;
    }

    if (!string.IsNullOrWhiteSpace(dto.Descripcion))
      permiso.Descripcion = dto.Descripcion;

    permiso.UpdatedAt = DateTime.Now;

    try
    {
      await _context.SaveChangesAsync();
      return (permiso, null);
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _context.Permisos.AnyAsync(p => p.Id == id))
        return (null, "Permiso no encontrado (error de concurrencia).");
      throw;
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, permiso));
    }
  }

  public async Task<(bool deleted, string? errorMessage)> DeletePermisoAsync(int id)
  {
    var permiso = await _context.Permisos.FindAsync(id);
    if (permiso == null) return (false, "El permiso no existe.");

    try
    {
      _context.Permisos.Remove(permiso);
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (DbUpdateException)
    {
      return (false, "No se puede eliminar el permiso porque está asignado a un rol.");
    }
    catch (Exception)
    {
      return (false, "Error interno al eliminar el permiso.");
    }
  }

  private string InterpretarDbError(DbUpdateException ex, Permiso permiso)
  {
    if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
        ex.InnerException.Message.Contains("'nombre'", StringComparison.OrdinalIgnoreCase))
    {
      return $"El nombre de permiso '{permiso.Nombre}' ya está en uso.";
    }
    return "Error al guardar el permiso. Asegúrate de que los datos sean únicos y válidos.";
  }
}