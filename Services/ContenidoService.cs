using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class ContenidoService : IContenidoService
{
  private readonly TurneroDbContext _context;
  private readonly IWebHostEnvironment _env;

  public ContenidoService(TurneroDbContext context, IWebHostEnvironment env)
  {
    _context = context;
    _env = env;
  }

  public async Task<IEnumerable<Contenido>> GetContenidosAsync()
  {
    return await _context.Contenidos.ToListAsync();
  }

  public async Task<Contenido?> GetContenidoAsync(uint id)
  {
    return await _context.Contenidos.FindAsync(id);
  }

  public async Task<(List<Contenido> contenidos, string? errorMessage)> CreateContenidosAsync(List<IFormFile> archivos, List<bool> activos)
  {
    if (archivos.Count != activos.Count)
      return (new(), "Cantidad de archivos y estados no coincide.");

    Console.WriteLine($"Recibidos {archivos.Count} archivos y {activos.Count} estados.");

    var contenidos = new List<Contenido>();

    for (int i = 0; i < archivos.Count; i++)
    {
      var archivo = archivos[i];
      var activa = activos[i];

      Console.WriteLine($"Archivo {i + 1}:");
      Console.WriteLine($"  Nombre: {archivo.FileName}");
      Console.WriteLine($"  Tamaño: {archivo.Length} bytes");
      Console.WriteLine($"  Tipo MIME: {archivo.ContentType}");
      Console.WriteLine($"  Activo: {activa}");

      if (!ContenidoValidator.EsExtensionValida(archivo.FileName))
        return (new(), $"Archivo no permitido: {archivo.FileName}");

      var nombreNormalizado = NormalizarVariables.NormalizarNombreArchivo(archivo.FileName);
      var rutaFinal = Path.Combine(_env.ContentRootPath, "ContenidoNAS", nombreNormalizado);

      // Comentado: no guardar archivo ni crear stream
      // using var stream = new FileStream(rutaFinal, FileMode.Create);
      // await archivo.CopyToAsync(stream);

      var contenido = new Contenido
      {
        Nombre = nombreNormalizado,
        Ruta = rutaFinal,
        Tipo = ContenidoValidator.ObtenerTipo(nombreNormalizado),
        Activa = activa,
        Fecha = DateTime.Now
      };

      contenidos.Add(contenido);
    }

    // Comentado: no guardar en base de datos
    // _context.Contenidos.AddRange(contenidos);
    // await _context.SaveChangesAsync();

    return (contenidos, null);
  }

  public async Task<(Contenido? contenido, string? errorMessage)> UpdateContenidoAsync(uint id, ContenidoActualizarDto dto)
  {
    var contenido = await _context.Contenidos.FindAsync(id);
    if (contenido == null)
      return (null, "Contenido no encontrado.");

    if (dto.Activa.HasValue)
      contenido.Activa = dto.Activa.Value;

    await _context.SaveChangesAsync();
    return (contenido, null);
  }

  public async Task<(bool deleted, string? errorMessage)> DeleteContenidoAsync(uint id)
  {
    var contenido = await _context.Contenidos.FindAsync(id);
    if (contenido == null)
      return (false, "Contenido no encontrada.");

    try
    {
      _context.Contenidos.Remove(contenido);
      await _context.SaveChangesAsync();
      return (true, null);
    }
    catch (Exception)
    {
      return (false, "Error interno al eliminar el contenido.");
    }
  }
}
