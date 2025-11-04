using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Utils;
using TurneroApi.Validation;
using Microsoft.Extensions.Options;
using TurneroApi.Config;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace TurneroApi.Services;

public class ContenidoService : IContenidoService
{
  private readonly TurneroDbContext _context;
  private readonly IWebHostEnvironment _env;
  private readonly RutasConfig _rutas;

  public ContenidoService(TurneroDbContext context, IWebHostEnvironment env, IOptions<RutasConfig> rutas)
  {
    _context = context;
    _env = env;
    _rutas = rutas.Value;
  }

  public async Task<IEnumerable<Contenido>> GetContenidosAsync()
  {
    var rutaBase = _rutas.CarpetaContenidos;
    var archivosFisicos = Directory.GetFiles(rutaBase)
      .Select(Path.GetFileName)
      .Where(nombre => nombre != null)
      .ToHashSet();

    var registrosDb = await _context.Contenidos.ToListAsync();

    // Eliminar registros que no tienen archivo físico
    var registrosSinArchivo = registrosDb
      .Where(c => c.Nombre != null && !archivosFisicos.Contains(c.Nombre))
      .ToList();

    foreach (var registro in registrosSinArchivo)
    {
      _context.Contenidos.Remove(registro);
    }

    // Eliminar archivos físicos que no tienen registro
    var nombresDb = registrosDb
      .Select(c => c.Nombre)
      .Where(nombre => nombre != null)
      .ToHashSet();

    var archivosSinRegistro = archivosFisicos
      .Where(nombre => nombre != null && !nombresDb.Contains(nombre))
      .ToList();

    foreach (var nombre in archivosSinRegistro)
    {
      var ruta = Path.Combine(rutaBase, nombre!);
      try
      {
        System.IO.File.Delete(ruta);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"⚠️ Error al borrar archivo huérfano '{nombre}': {ex.Message}");
      }
    }

    // Guardar cambios si hubo eliminaciones
    if (registrosSinArchivo.Any() || archivosSinRegistro.Any())
      await _context.SaveChangesAsync();

    // Devolver solo los registros que tienen archivo físico
    var contenidosSincronizados = registrosDb
      .Where(c => c.Nombre != null && archivosFisicos.Contains(c.Nombre))
      .OrderByDescending(c => c.Fecha)
      .ToList();

    return contenidosSincronizados;
  }

  public async Task<Contenido?> GetContenidoAsync(uint id)
  {
    return await _context.Contenidos.FindAsync(id);
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

      // 🔒 Validar si ya existe en la base de datos
      var existeEnDb = await _context.Contenidos.AnyAsync(c => c.Nombre == nombreNormalizado);
      if (existeEnDb)
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

      // Generar miniatura si es video
      if (ContenidoValidator.EsVideo(nombreNormalizado))
      {
        var rutaMiniatura = Path.Combine(rutaMiniaturas, Path.ChangeExtension(nombreNormalizado, ".jpg"));
        if (!System.IO.File.Exists(rutaMiniatura))
        {
          var comando = $"ffmpeg -i \"{rutaFinal}\" -ss 00:00:01 -vframes 1 \"{rutaMiniatura}\"";
          try
          {
            var proceso = new System.Diagnostics.Process
            {
              StartInfo = new System.Diagnostics.ProcessStartInfo
              {
                FileName = "/bin/bash",
                Arguments = $"-c \"{comando}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
              }
            };

            proceso.Start();
            string salida = await proceso.StandardOutput.ReadToEndAsync();
            string errores = await proceso.StandardError.ReadToEndAsync();
            proceso.WaitForExit();

            await File.AppendAllTextAsync("/var/log/turnero/miniaturas.log", $"[{DateTime.Now}] {errores}\n");
          }
          catch (Exception ex)
          {
            Console.WriteLine($"⚠️ Error al ejecutar ffmpeg: {ex.Message}");
          }

        }
      }

      // Generar miniatura si es imagen
      if (ContenidoValidator.EsImagen(nombreNormalizado))
      {
        var rutaMiniatura = Path.Combine(rutaMiniaturas, nombreNormalizado);
        try
        {
          using var image = await SixLabors.ImageSharp.Image.LoadAsync(archivo.OpenReadStream());
          image.Mutate(x => x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
          {
            Mode = SixLabors.ImageSharp.Processing.ResizeMode.Max,
            Size = new SixLabors.ImageSharp.Size(200, 0)
          }));
          await image.SaveAsJpegAsync(rutaMiniatura);
        }
        catch (Exception ex)
        {
          Console.WriteLine($"⚠️ Error al generar miniatura de imagen: {ex.Message}");
        }
      }

      contenidos.Add(new Contenido
      {
        Nombre = nombreNormalizado,
        Ruta = rutaFinal,
        Tipo = ContenidoValidator.ObtenerTipo(nombreNormalizado),
        Activa = activa,
        Fecha = DateTime.Now
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

    if (dto.Activa.HasValue)
      contenido.Activa = dto.Activa.Value;

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
      // Eliminar archivo físico
      if (System.IO.File.Exists(rutaContenido))
      {
        System.IO.File.Delete(rutaContenido);
      }
      else
      {
        Console.WriteLine($"⚠️ Archivo no encontrado: {rutaContenido}");
      }

      // Eliminar miniatura
      if (System.IO.File.Exists(rutaMiniatura))
      {
        System.IO.File.Delete(rutaMiniatura);
      }
      else
      {
        Console.WriteLine($"⚠️ Miniatura no encontrada: {rutaMiniatura}");
      }

      // Eliminar registro en base
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
