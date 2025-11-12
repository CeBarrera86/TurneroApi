using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models;

namespace TurneroApi.Validation
{
  public static class EstadoValidator
  {
    public static async Task<string?> ValidarLetraUnicaAsync(TurneroDbContext context, string letra, int? id = null) // ← int
    {
      var existe = await context.Estados.AnyAsync(e => e.Letra == letra && (!id.HasValue || e.Id != id.Value));

      return existe ? $"Ya existe un estado con la letra '{letra}'. La letra debe ser única." : null;
    }
  }
}