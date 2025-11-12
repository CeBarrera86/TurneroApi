using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.RolPermiso;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class RolPermisoService : IRolPermisoService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;

  public RolPermisoService(TurneroDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<RolPermiso>> GetRolPermisosAsync()
  {
    return await _context.RolPermisos
        .Include(rp => rp.Rol)
        .Include(rp => rp.Permiso)
        .AsNoTracking()
        .ToListAsync();
  }

  public async Task<RolPermiso?> GetRolPermisoAsync(int rolId, int permisoId)
  {
    return await _context.RolPermisos
        .Include(rp => rp.Rol)
        .Include(rp => rp.Permiso)
        .AsNoTracking()
        .FirstOrDefaultAsync(rp => rp.RolId == rolId && rp.PermisoId == permisoId);
  }

  public async Task<(RolPermiso? rolPermiso, string? errorMessage)> CreateRolPermisoAsync(RolPermisoCrearDto dto)
  {
    var error = await RolPermisoValidator.ValidateAsync(_context, dto.RolId, dto.PermisoId);
    if (error != null) return (null, error);

    var rolPermiso = new RolPermiso
    {
      RolId = dto.RolId,
      PermisoId = dto.PermisoId,
    };

    _context.RolPermisos.Add(rolPermiso);
    try
    {
      await _context.SaveChangesAsync();
      return (rolPermiso, null);
    }
    catch (Exception ex)
    {
      return (null, $"Error al crear la relación rol-permiso: {ex.Message}");
    }
  }

  public async Task<(bool deleted, string? errorMessage)> DeleteRolPermisoAsync(int rolId, int permisoId)
  {
    var rolPermiso = await _context.RolPermisos.FindAsync(rolId, permisoId);
    if (rolPermiso == null) return (false, "La relación rol-permiso no existe.");

    try
    {
      _context.RolPermisos.Remove(rolPermiso);
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (Exception ex)
    {
      return (false, $"Error al eliminar la relación rol-permiso: {ex.Message}");
    }
  }
}
