using System.Security.Claims;

namespace TurneroApi.Helpers
{
  public static class PermisoHelper
  {
    public static bool TienePermiso(ClaimsPrincipal user, string permiso)
    {
      return user.HasClaim("permiso", permiso);
    }

    public static List<string> ObtenerPermisos(ClaimsPrincipal user)
    {
      return user.Claims.Where(c => c.Type == "permiso").Select(c => c.Value).ToList();
    }

    public static bool TieneAlguno(ClaimsPrincipal user, params string[] permisos)
    {
      return permisos.Any(p => user.HasClaim("permiso", p));
    }
  }
}