using TurneroApi.Interfaces;

namespace TurneroApi.Services;

public class UrlBuilderService : IUrlBuilderService
{
  public string ConstruirUrlMiniatura(string nombre, string tipo)
  {
    if (tipo == "video")
    {
      var ext = Path.GetExtension(nombre);
      var baseName = Path.GetFileNameWithoutExtension(nombre);
      return $"/api/contenido/miniaturas/{baseName}.jpg";
    }

    return $"/api/contenido/miniaturas/{nombre}";
  }

  public string ConstruirUrlArchivo(string nombre)
  {
    return $"/api/contenido/archivos/{nombre}";
  }
}
