using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TurneroApi.Config;
using TurneroApi.Data;
using TurneroApi.DTOs.Contenido;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class ContenidoService : IContenidoService
{
  private readonly TurneroDbContext _context;
  private readonly RutasConfig _rutas;
  private readonly IMiniaturaService _miniaturaService;
  private readonly IMapper _mapper;

  public ContenidoService(
      TurneroDbContext context,
      IOptions<RutasConfig> rutas,
      IMiniaturaService miniaturaService,
      IMapper mapper)
  {
    _context = context;
    _rutas = rutas.Value;
    _miniaturaService = miniaturaService;
    _mapper = mapper;
  }

  public async Task<PagedResult<ContenidoDto>> GetContenidosAsync(int page, int pageSize)
  {
    var rutaBase = _rutas.CarpetaContenidos;
    var archivosFisicos = Directory.GetFiles(rutaBase)
        .Select(Path.GetFileName)
        .Where(nombre => nombre != null)
        .ToHashSet();

    var registrosDb = await _context.Contenidos.ToListAsync();

    // Eliminar registros sin archivo físico
    var registrosSinArchivo = registrosDb
        .Where(c => c.Nombre != null && !archivosFisicos.Contains(c.Nombre))
        .ToList();

    foreach (var registro in registrosSinArchivo)
      _context.Contenidos.Remove(registro);

    // Eliminar archivos huérfanos
    var nombresDb = registrosDb.Select(c => c.Nombre).ToHashSet();
    var archivosSinRegistro = archivosFisicos.Where(nombre => !nombresDb.Contains(nombre!)).ToList();

    foreach (var nombre in archivosSinRegistro)
    {
      var ruta = Path.Combine(rutaBase, nombre!);
      try { System.IO.File.Delete(ruta); }
      catch (Exception ex) { Console.WriteLine($"⚠️ Error al borrar archivo '{nombre}': {ex.Message}"); }
    }

    if (registrosSinArchivo.Any() || archivosSinRegistro.Any())
      await _context.SaveChangesAsync();

    var items = registrosDb
        .Where(c => c.Nombre != null && archivosFisicos.Contains(c.Nombre))
        .OrderByDescending(c => c.CreatedAt)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
        .ToList();

    var total = registrosDb.Count(c => c.Nombre != null && archivosFisicos.Contains(c.Nombre));
    var dtoItems = _mapper.Map<List<ContenidoDto>>(items);
    return new PagedResult<ContenidoDto>(dtoItems, total);
  }

  public async Task<ContenidoDto?> GetContenidoAsync(uint id)
  {
    var contenido = await _context.Contenidos.FindAsync(id);
    return contenido == null ? null : _mapper.Map<ContenidoDto>(contenido);
  }

  public async Task<(List<Contenido> contenidos, string? errorMessage)> CreateContenidosAsync(List<IFormFile> archivos, List<bool> activos)
  {
    if (archivos.Count != activos.Count)
      return (new(), "Cantidad de archivos y estados no coincide.");

    var contenidos = new List<Contenido>();
    var rutaBase = _rutas.CarpetaContenidos;
    var rutaMiniaturas = _rutas.CarpetaMiniaturas;

    for (int i = 0; i < archivos.Count; i++)
    {
      var archivo = archivos[i];
      var activa = activos[i];

      if (!ContenidoValidator.EsExtensionValida(archivo.FileName))
        return (new(), $"Archivo no permitido: {archivo.FileName}");

      var nombreNormalizado = NormalizarVariables.NormalizarNombreArchivo(archivo.FileName);

      if (await _context.Contenidos.AnyAsync(c => c.Nombre == nombreNormalizado))
        return (new(), $"Ya existe un contenido con el nombre: {nombreNormalizado}");

      var rutaFinal = Path.Combine(rutaBase, nombreNormalizado);

      try
      {
        using var stream = new FileStream(rutaFinal, FileMode.Create);
        await archivo.CopyToAsync(stream);
      }
      catch (Exception ex)
      {
        return (new(), $"No se pudo guardar el archivo: {archivo.FileName} ({ex.Message})");
      }

      // Miniaturas
      if (ContenidoValidator.EsVideo(nombreNormalizado))
      {
        var rutaMiniatura = Path.Combine(rutaMiniaturas, Path.ChangeExtension(nombreNormalizado, ".jpg"));
        if (!System.IO.File.Exists(rutaMiniatura))
          await _miniaturaService.GenerarMiniaturaVideoAsync(rutaFinal, rutaMiniatura);
      }

      if (ContenidoValidator.EsImagen(nombreNormalizado))
      {
        var rutaMiniatura = Path.Combine(rutaMiniaturas, nombreNormalizado);
        await _miniaturaService.GenerarMiniaturaImagenAsync(archivo, rutaMiniatura);
      }

      contenidos.Add(new Contenido
      {
        Nombre = nombreNormalizado,
        Ruta = nombreNormalizado, // guardamos nombre relativo
        Tipo = ContenidoValidator.ObtenerTipo(nombreNormalizado),
        Activo = activa,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      });
    }

    try
    {
      _context.Contenidos.AddRange(contenidos);
      await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      return (new(), $"Error al registrar contenidos: {ex.Message}");
    }

    return (contenidos, null);
  }

  public async Task<(Contenido? contenido, string? errorMessage)> UpdateContenidoAsync(uint id, ContenidoActualizarDto dto)
  {
    var contenido = await _context.Contenidos.FindAsync(id);
    if (contenido == null)
      return (null, "Contenido no encontrado.");

    if (dto.Activo.HasValue)
      contenido.Activo = dto.Activo.Value;

    contenido.UpdatedAt = DateTime.Now;

    await _context.SaveChangesAsync();
    return (contenido, null);
  }

  public async Task<(bool deleted, string? errorMessage)> DeleteContenidoAsync(uint id)
  {
    var contenido = await _context.Contenidos.FindAsync(id);
    if (contenido == null)
      return (false, "Contenido no encontrado.");

    var rutaContenido = Path.Combine(_rutas.CarpetaContenidos, contenido.Nombre);
    var nombreMiniatura = contenido.Tipo == "video"
        ? Path.ChangeExtension(contenido.Nombre, ".jpg")
        : contenido.Nombre;
    var rutaMiniatura = Path.Combine(_rutas.CarpetaMiniaturas, nombreMiniatura);

    try
    {
      if (System.IO.File.Exists(rutaContenido))
        System.IO.File.Delete(rutaContenido);

      if (System.IO.File.Exists(rutaMiniatura))
        System.IO.File.Delete(rutaMiniatura);

      _context.Contenidos.Remove(contenido);
      await _context.SaveChangesAsync();

      return (true, null);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"❌ Error al eliminar contenido ID={id}: {ex.Message}");
      return (false, "Error interno al eliminar el contenido.");
    }
  }
}