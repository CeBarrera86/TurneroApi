using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class PuestoValidator
{
  public static async Task<string?> ValidateUsuarioExisteAsync(TurneroDbContext context, int usuarioId)
  {
    var exists = await context.Usuarios.AnyAsync(u => u.Id == usuarioId);
    return exists ? null : $"El UsuarioId '{usuarioId}' no es válido.";
  }

  public static async Task<string?> ValidateMostradorExisteAsync(TurneroDbContext context, int mostradorId)
  {
    var exists = await context.Mostradores.AnyAsync(m => m.Id == mostradorId);
    return exists ? null : $"El MostradorId '{mostradorId}' no es válido.";
  }

  public static async Task<string?> ValidateDuplicadoAsync(TurneroDbContext context, int usuarioId, int mostradorId, int? excludeId = null)
  {
    var exists = await context.Puestos
        .AnyAsync(p => p.UsuarioId == usuarioId && p.MostradorId == mostradorId && (!excludeId.HasValue || p.Id != excludeId.Value));
    return exists ? "Ya existe un puesto con ese usuario y mostrador." : null;
  }
}