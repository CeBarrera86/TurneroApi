namespace TurneroApi.Utils;

public static class MimeHelper
{
  private static readonly Dictionary<string, string> _mimeTypes = new()
  {
    { ".jpg", "image/jpeg" },
    { ".jpeg", "image/jpeg" },
    { ".png", "image/png" },
    { ".gif", "image/gif" },
    { ".mp4", "video/mp4" },
    { ".webm", "video/webm" },
    { ".avi", "video/x-msvideo" }
  };

  public static string GetMimeType(string fileName)
  {
    var ext = Path.GetExtension(fileName).ToLower();
    return _mimeTypes.TryGetValue(ext, out var mime) ? mime : "application/octet-stream";
  }
}