using TurneroApi.Config;
using TurneroApi.Interfaces;
using Microsoft.Extensions.Options;

namespace TurneroApi.Services;

public class ArchivoService : IArchivoService
{
  private readonly RutasConfig _rutas;

  public ArchivoService(IOptions<RutasConfig> rutas)
  {
    _rutas = rutas.Value;
  }

  public string ObtenerRutaMiniatura(string nombre)
  {
    var ext = Path.GetExtension(nombre).ToLower();
    var esImagen = new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext);

    var nombreFinal = esImagen
      ? nombre
      : Path.ChangeExtension(nombre, ".jpg");

    return Path.Combine(_rutas.CarpetaMiniaturas, nombreFinal);
  }

  public string ObtenerRutaArchivo(string nombre)
  {
    return Path.Combine(_rutas.CarpetaContenidos, nombre);
  }
}