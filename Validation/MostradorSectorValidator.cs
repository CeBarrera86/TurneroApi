using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class MostradorSectorValidator
{
  public static async Task<string?> ValidateExistenciaAsync(TurneroDbContext context, int mostradorId, int sectorId)
  {
    var existe = await context.MostradorSectores
        .AnyAsync(ms => ms.MostradorId == mostradorId && ms.SectorId == sectorId);
    return existe ? "La asociación mostrador-sector ya existe." : null;
  }

  public static async Task<string?> ValidateIdsAsync(TurneroDbContext context, int mostradorId, int sectorId)
  {
    var mostradorExiste = await context.Mostradores.AnyAsync(m => m.Id == mostradorId);
    if (!mostradorExiste) return $"El MostradorId '{mostradorId}' no existe.";

    var sectorExiste = await context.Sectores.AnyAsync(s => s.Id == sectorId);
    if (!sectorExiste) return $"El SectorId '{sectorId}' no existe.";

    return null;
  }
}