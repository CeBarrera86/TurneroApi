using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class UsuarioValidator
{
  public static async Task<string?> ValidateUsernameAsync(TurneroDbContext context, string? username, uint? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(username)) return null;

    var exists = await context.Usuarios.AnyAsync(u => u.Username == username && (!excludeId.HasValue || u.Id != excludeId.Value));

    return exists ? $"El nombre de usuario '{username}' ya está en uso." : null;
  }

  public static async Task<string?> ValidateRolIdAsync(TurneroDbContext context, uint rolId)
  {
    if (rolId == 0) return "El RolId proporcionado no es válido (no puede ser 0).";

    var exists = await context.Roles.AnyAsync(r => r.Id == rolId);
    return exists ? null : $"El RolId '{rolId}' no es válido.";
  }
}