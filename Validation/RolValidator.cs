using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class RolValidator
{
  public static async Task<string?> ValidateTipoAsync(TurneroDbContext context, string? tipo, uint? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(tipo)) return null;

    var exists = await context.Roles
        .AnyAsync(r => r.Tipo == tipo && (!excludeId.HasValue || r.Id != excludeId.Value));

    return exists ? $"El tipo de rol '{tipo}' ya está en uso." : null;
  }
}