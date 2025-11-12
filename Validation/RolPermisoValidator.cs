using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;

namespace TurneroApi.Validation;

public static class RolPermisoValidator
{
  public static async Task<string?> ValidateAsync(TurneroDbContext context, int rolId, int permisoId)
  {
    var exists = await context.RolPermisos
        .AnyAsync(rp => rp.RolId == rolId && rp.PermisoId == permisoId);

    return exists ? "La relación rol-permiso ya existe." : null;
  }
}
