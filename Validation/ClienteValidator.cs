using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class ClienteValidator
{
  public static async Task<string?> ValidateDniAsync(TurneroDbContext context, string dni, ulong? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(dni))
      return "El DNI no puede estar vacío.";

    var exists = await context.Clientes
        .AnyAsync(c => c.Dni == dni && (!excludeId.HasValue || c.Id != excludeId.Value));

    return exists ? $"El DNI '{dni}' ya está en uso." : null;
  }
}
