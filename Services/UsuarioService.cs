using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class UsuarioService : IUsuarioService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;

  public UsuarioService(TurneroDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
  {
    return await _context.Usuarios.Include(u => u.RolNavigation).ToListAsync();
  }

  public async Task<Usuario?> GetUsuarioAsync(uint id)
  {
    return await _context.Usuarios.Include(u => u.RolNavigation).FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<(Usuario? usuario, string? errorMessage)> CreateUsuarioAsync(Usuario usuario)
  {
    usuario.Nombre = NormalizarVariables.NormalizeNombre(usuario.Nombre) ?? string.Empty;
    usuario.Apellido = NormalizarVariables.NormalizeNombre(usuario.Apellido) ?? string.Empty;
    usuario.Username = NormalizarVariables.NormalizeUsername(usuario.Username) ?? string.Empty;

    var usernameError = await UsuarioValidator.ValidateUsernameAsync(_context, usuario.Username);
    if (usernameError != null) return (null, usernameError);

    var rolError = await UsuarioValidator.ValidateRolIdAsync(_context, usuario.RolId);
    if (rolError != null) return (null, rolError);

    _context.Usuarios.Add(usuario);
    try
    {
      await _context.SaveChangesAsync();
      await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
      return (usuario, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, usuario));
    }
  }

  public async Task<(Usuario? usuario, string? errorMessage)> UpdateUsuarioAsync(uint id, UsuarioActualizarDto dto)
  {
    var usuario = await _context.Usuarios.FindAsync(id);
    if (usuario == null) return (null, "Usuario no encontrado.");

    _mapper.Map(dto, usuario);

    if (dto.Nombre != null)
      usuario.Nombre = NormalizarVariables.NormalizeNombre(dto.Nombre) ?? string.Empty;

    if (dto.Apellido != null)
      usuario.Apellido = NormalizarVariables.NormalizeNombre(dto.Apellido) ?? string.Empty;

    if (dto.Username != null)
      usuario.Username = NormalizarVariables.NormalizeUsername(dto.Username) ?? string.Empty;

    var usernameError = await UsuarioValidator.ValidateUsernameAsync(_context, usuario.Username, id);
    if (usernameError != null) return (null, usernameError);

    var rolError = await UsuarioValidator.ValidateRolIdAsync(_context, usuario.RolId);
    if (rolError != null) return (null, rolError);

    try
    {
      await _context.SaveChangesAsync();
      await _context.Entry(usuario).Reference(u => u.RolNavigation).LoadAsync();
      return (usuario, null);
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _context.Usuarios.AnyAsync(u => u.Id == id))
        return (null, "Usuario no encontrado (error de concurrencia).");
      throw;
    }
    catch (DbUpdateException ex)
    {
      return (null, InterpretarDbError(ex, usuario));
    }
  }

  public async Task<bool> DeleteUsuarioAsync(uint id)
  {
    var usuario = await _context.Usuarios.FindAsync(id);
    if (usuario == null) return false;

    if (await _context.Historiales.AnyAsync(h => h.UsuarioId == id)) return false;
    if (await _context.Puestos.AnyAsync(p => p.UsuarioId == id)) return false;

    _context.Usuarios.Remove(usuario);
    await _context.SaveChangesAsync();
    return true;
  }

  private string InterpretarDbError(DbUpdateException ex, Usuario usuario)
  {
    if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
    {
      if (ex.InnerException.Message.Contains("'username'", StringComparison.OrdinalIgnoreCase))
        return $"El nombre de usuario '{usuario.Username}' ya está en uso. (DB Error)";
    }
    return "Error al guardar el usuario. Verificá que los datos sean válidos y únicos.";
  }
}