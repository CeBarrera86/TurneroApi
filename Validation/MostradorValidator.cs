using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class MostradorValidator
{
  public static async Task<string?> ValidateIpAsync(TurneroDbContext context, string? ip, uint? excludeId = null)
  {
    if (string.IsNullOrWhiteSpace(ip)) return null;

    var exists = await context.Mostradores.AnyAsync(m => m.Ip == ip && (!excludeId.HasValue || m.Id != excludeId.Value));

    return exists ? $"La IP '{ip}' ya está en uso." : null;
  }

  public static async Task<string?> ValidateNumeroUnicoAsync(TurneroDbContext context, uint sectorId, uint numero, uint? excludeId = null)
  {
    var exists = await context.Mostradores.AnyAsync(m => m.SectorId == sectorId && m.Numero == numero && (!excludeId.HasValue || m.Id != excludeId.Value));

    return exists ? $"Ya existe un mostrador número {numero} en el sector {sectorId}." : null;
  }

  public static async Task<string?> ValidateSectorIdAsync(TurneroDbContext context, uint sectorId)
  {
    var exists = await context.Sectores.AnyAsync(s => s.Id == sectorId);
    return exists ? null : $"El SectorId '{sectorId}' no es válido.";
  }
}