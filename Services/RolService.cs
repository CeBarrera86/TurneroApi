using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
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
    return await _context.Roles.ToListAsync();
  }

  public async Task<Rol?> GetRolAsync(uint id)
  {
    return await _context.Roles.FindAsync(id);
  }

  public async Task<(Rol? rol, string? errorMessage)> CreateRolAsync(Rol rol)
  {
    // --- Normalización ---
    rol.Tipo = NormalizarVariables.NormalizeTipoRol(rol.Tipo) ?? string.Empty;

    // --- Validación ---
    var tipoError = await RolValidator.ValidateTipoAsync(_context, rol.Tipo);
    if (tipoError != null) return (null, tipoError);

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

  public async Task<(Rol? rol, string? errorMessage)> UpdateRolAsync(uint id, RolActualizarDto dto)
  {
    var rol = await _context.Roles.FindAsync(id);
    if (rol == null) return (null, "Rol no encontrado.");

    var tipo = NormalizarVariables.NormalizeTipoRol(dto.Tipo) ?? string.Empty;
    if (tipo != rol.Tipo)
    {
      var tipoError = await RolValidator.ValidateTipoAsync(_context, tipo, id);
      if (tipoError != null) return (null, tipoError);
      rol.Tipo = tipo;
    }

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

  public async Task<(bool deleted, string? errorMessage)> DeleteRolAsync(uint id)
  {
    var rol = await _context.Roles.FindAsync(id);
    if (rol == null)
      return (false, "El rol no existe.");

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

  // --- Manejo de errores centralizado ---
  private string InterpretarDbError(DbUpdateException ex, Rol rol)
  {
    if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true &&
        ex.InnerException.Message.Contains("'tipo'", StringComparison.OrdinalIgnoreCase))
    {
      return $"El tipo de rol '{rol.Tipo}' ya está en uso. (DB Error)";
    }
    return "Error al guardar el rol. Asegúrate de que los datos sean únicos y válidos.";
  }
}