using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class MostradorValidator
{
  public static async Task<string?> ValidateIpAsync(TurneroDbContext context, string ip, int? excludeId = null)
  {
    var exists = await context.Mostradores
        .AnyAsync(m => m.Ip == ip && (!excludeId.HasValue || m.Id != excludeId.Value));
    return exists ? $"La IP '{ip}' ya está en uso." : null;
  }

  public static async Task<string?> ValidateNumeroIpUniqueAsync(TurneroDbContext context, int numero, string ip, int? excludeId = null)
  {
    var exists = await context.Mostradores
        .AnyAsync(m => m.Numero == numero && m.Ip == ip && (!excludeId.HasValue || m.Id != excludeId.Value));
    return exists ? $"Ya existe un mostrador con número {numero} y IP {ip}." : null;
  }
}