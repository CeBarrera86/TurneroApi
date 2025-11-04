using TurneroApi.DTOs;

namespace TurneroApi.Validation;

public static class ContenidoValidator
{
	private static readonly string[] extensionesPermitidas = { "jpg", "jpeg", "png", "gif", "mp4", "webm", "avi" };

	public static bool EsExtensionValida(string nombreArchivo)
	{
		var ext = Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
		return extensionesPermitidas.Contains(ext);
	}

	public static string ObtenerTipo(string nombreArchivo)
	{
		var ext = Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
		return new[] { "jpg", "jpeg", "png", "gif" }.Contains(ext) ? "imagen" : "video";
	}

	public static bool EsVideo(string nombreArchivo)
	{
		var ext = Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
		return new[] { "mp4", "webm", "avi" }.Contains(ext);
	}

	public static bool EsImagen(string nombreArchivo)
	{
		var ext = Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
		return new[] { "jpg", "jpeg", "png", "gif" }.Contains(ext);
	}
}
