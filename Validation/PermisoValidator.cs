using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class PermisoValidator
{
  public static async Task<string?> ValidateNombreAsync(TurneroDbContext context, string? nombre, int? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(nombre))
      return "El nombre del permiso no puede estar vacío.";

    var exists = await context.Permisos.AnyAsync(p => p.Nombre == nombre && (!excludeId.HasValue || p.Id != excludeId.Value));

    return exists ? $"El nombre de permiso '{nombre}' ya está en uso." : null;
  }
}
