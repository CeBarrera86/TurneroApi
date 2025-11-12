using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TurneroApi.Interfaces;

namespace TurneroApi.Services;

public class MiniaturaService : IMiniaturaService
{
  public async Task GenerarMiniaturaImagenAsync(IFormFile archivo, string destino)
  {
    using var image = await Image.LoadAsync(archivo.OpenReadStream());
    image.Mutate(x => x.Resize(new ResizeOptions
    {
      Mode = ResizeMode.Max,
      Size = new Size(200, 0)
    }));
    await image.SaveAsJpegAsync(destino);
  }

  public async Task GenerarMiniaturaVideoAsync(string rutaVideo, string destino)
  {
    var comando = $"ffmpeg -i \"{rutaVideo}\" -ss 00:00:01 -vframes 1 \"{destino}\"";
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