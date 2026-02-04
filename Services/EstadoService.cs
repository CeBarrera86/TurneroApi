using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Estado;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services
{
  public class EstadoService : IEstadoService
  {
    private readonly TurneroDbContext _context;
    private readonly IMapper _mapper;

    public EstadoService(TurneroDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PagedResult<EstadoDto>> GetEstadosAsync(int page, int pageSize)
    {
      var query = _context.Estados
        .AsNoTracking()
        .OrderBy(e => e.Id)
        .ProjectTo<EstadoDto>(_mapper.ConfigurationProvider);

      return await query.ToPagedResultAsync(page, pageSize);
    }

    public async Task<EstadoDto?> GetEstadoAsync(int id) // ← int en lugar de uint
    {
      return await _context.Estados
        .AsNoTracking()
        .Where(e => e.Id == id)
        .ProjectTo<EstadoDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }

    public async Task<(Estado? estado, string? errorMessage)> CreateEstadoAsync(Estado estado)
    {
      estado.Letra = NormalizarVariables.NormalizeLetraEstado(estado.Letra) ?? string.Empty;
      estado.Descripcion = NormalizarVariables.NormalizeDescripcionEstado(estado.Descripcion) ?? string.Empty;

      var errorLetra = await EstadoValidator.ValidarLetraUnicaAsync(_context, estado.Letra!);
      if (errorLetra != null)
      {
        return (null, errorLetra);
      }

      _context.Estados.Add(estado);
      try
      {
        await _context.SaveChangesAsync();
        return (estado, null);
      }
      catch (DbUpdateException ex)
      {
        return (null, InterpretarDbError(ex, estado));
      }
    }

    public async Task<(Estado? estado, string? errorMessage)> UpdateEstadoAsync(int id, EstadoActualizarDto dto) // ← int
    {
      var estado = await _context.Estados.FindAsync(id);
      if (estado == null)
      {
        return (null, "Estado no encontrado.");
      }

      if (!string.IsNullOrWhiteSpace(dto.Letra))
      {
        string cleanedLetra = NormalizarVariables.NormalizeLetraEstado(dto.Letra) ?? string.Empty;
        if (cleanedLetra != estado.Letra)
        {
          var errorLetra = await EstadoValidator.ValidarLetraUnicaAsync(_context, cleanedLetra, id);
          if (errorLetra != null)
          {
            return (null, errorLetra);
          }
          estado.Letra = cleanedLetra;
        }
      }

      if (!string.IsNullOrWhiteSpace(dto.Descripcion))
      {
        string cleanedDescripcion = NormalizarVariables.NormalizeDescripcionEstado(dto.Descripcion) ?? string.Empty;
        if (cleanedDescripcion != estado.Descripcion)
        {
          estado.Descripcion = cleanedDescripcion;
        }
      }

      try
      {
        await _context.SaveChangesAsync();
        return (estado, null);
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!await _context.Estados.AnyAsync(e => e.Id == id))
        {
          return (null, "Estado no encontrado (error de concurrencia).");
        }
        throw;
      }
      catch (DbUpdateException ex)
      {
        return (null, InterpretarDbError(ex, estado));
      }
    }

    public async Task<(bool deleted, string? errorMessage)> DeleteEstadoAsync(int id) // ← int
    {
      var estado = await _context.Estados.FindAsync(id);
      if (estado == null)
      {
        return (false, "El estado no existe.");
      }

      _context.Estados.Remove(estado);
      await _context.SaveChangesAsync();
      return (true, null);
    }

    // --- Manejo de errores de base de datos ---
    private string InterpretarDbError(DbUpdateException ex, Estado estado)
    {
      if (ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true)
      {
        if (ex.InnerException.Message.Contains("'letra'", StringComparison.OrdinalIgnoreCase))
          return $"La letra '{estado.Letra}' ya está en uso. (DB Error)";
      }
      return "Error al guardar el estado. Asegúrate de que los datos sean únicos y válidos.";
    }
  }
}
