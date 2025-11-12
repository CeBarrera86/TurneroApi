using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Rol;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Validation;

namespace TurneroApi.Services;

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
    return await _context.Roles.AsNoTracking().ToListAsync();
  }

  public async Task<Rol?> GetRolAsync(int id)
  {
    return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
  }

  public async Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol)
  {
    var nombreError = await RolValidator.ValidateNombreAsync(_context, rol.Nombre);
    if (nombreError != null) return (null, nombreError);

    _context.Roles.Add(rol);
    try
    {
      await _context.SaveChangesAsync();
      return (rol, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, rol));
    }
  }

  public async Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(int id, RolActualizarDto dto)
  {
    var rol = await _context.Roles.FindAsync(id);
    if (rol == null) return (null, "Rol no encontrado.");

    if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre != rol.Nombre)
    {
      var nombreError = await RolValidator.ValidateNombreAsync(_context, dto.Nombre, id);
      if (nombreError != null) return (null, nombreError);
      rol.Nombre = dto.Nombre;
    }

    rol.UpdatedAt = DateTime.Now;

    try
    {
      await _context.SaveChangesAsync();
      return (rol, null);
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _context.Roles.AnyAsync(r => r.Id == id))
        return (null, "Rol no encontrado (error de concurrencia).");
      throw;
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, rol));
    }
  }

  public async Task<(bool deleted, string? errorMessage)> DeleteRolAsync(int id)
  {
    var rol = await _context.Roles.FindAsync(id);
    if (rol == null) return (false, "El rol no existe.");

    try
    {
      _context.Roles.Remove(rol);
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (DbUpdateException)
    {
      return (false, "No se puede eliminar el rol porque está asignado a una persona.");
    }
    catch (Exception)
    {
      return (false, "Error interno al eliminar el rol.");
    }
  }

  private string InterpretarDbError(DbUpdateException ex, Rol rol)
  {
    if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
        ex.InnerException.Message.Contains("'nombre'", StringComparison.OrdinalIgnoreCase))
    {
      return $"El nombre de rol '{rol.Nombre}' ya está en uso.";
    }
    return "Error al guardar el rol. Asegúrate de que los datos sean únicos y válidos.";
  }
}