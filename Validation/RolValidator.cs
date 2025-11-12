using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class RolValidator
{
  public static async Task<string?> ValidateNombreAsync(TurneroDbContext context, string? nombre, int? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(nombre))
      return "El nombre del rol no puede estar vacío.";

    var exists = await context.Roles
        .AnyAsync(r => r.Nombre == nombre && (!excludeId.HasValue || r.Id != excludeId.Value));

    return exists ? $"El nombre de rol '{nombre}' ya está en uso." : null;
  }
}