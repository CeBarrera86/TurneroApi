namespace TurneroApi.Interfaces;

public interface IMiniaturaService
{
  Task GenerarMiniaturaImagenAsync(IFormFile archivo, string destino);
  Task GenerarMiniaturaVideoAsync(string rutaVideo, string destino);
}
