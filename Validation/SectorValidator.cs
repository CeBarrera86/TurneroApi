using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class SectorValidator
{
  public static async Task<string?> ValidateLetraAsync(TurneroDbContext context, string? letra, int? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(letra)) return null;

    var exists = await context.Sectores
        .AnyAsync(s => s.Letra == letra && (!excludeId.HasValue || s.Id != excludeId.Value));

    return exists ? $"La letra '{letra}' ya está en uso." : null;
  }

  public static async Task<string?> ValidateNombreAsync(TurneroDbContext context, string? nombre, int? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(nombre)) return null;

    var exists = await context.Sectores
        .AnyAsync(s => s.Nombre == nombre && (!excludeId.HasValue || s.Id != excludeId.Value));

    return exists ? $"El nombre '{nombre}' ya está en uso." : null;
  }

  public static async Task<string?> ValidatePadreIdAsync(TurneroDbContext context, int? padreId, int selfId)
  {
    if (!padreId.HasValue) return null;
    if (padreId.Value == selfId) return "Un sector no puede ser su propio padre.";

    var exists = await context.Sectores.AnyAsync(s => s.Id == padreId.Value);
    return exists ? null : $"El PadreId '{padreId.Value}' no es válido.";
  }
}