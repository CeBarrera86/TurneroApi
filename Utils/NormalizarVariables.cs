using System.Globalization;
using System.Text.RegularExpressions;

namespace TurneroApi.Utils;

public static class NormalizarVariables
{
  // --- LETRA ---
  public static string? NormalizeLetraSector(string? input) => string.IsNullOrWhiteSpace(input) ? null : input.Trim().Replace(" ", "").ToUpperInvariant();
  public static string? NormalizeLetraTicket(string? input) => string.IsNullOrWhiteSpace(input) ? null : input.Trim().Replace(" ", "").ToUpperInvariant();
  public static string? NormalizeLetraEstado(string? input) => string.IsNullOrWhiteSpace(input) ? null : input.Trim().Replace(" ", "").ToUpperInvariant();

  // --- DESCRIPCIÓN ---
  public static string? NormalizeDescripcionSector(string? input)
  {
    if (string.IsNullOrWhiteSpace(input)) return null;
    var textInfo = CultureInfo.InvariantCulture.TextInfo;
    var cleaned = textInfo.ToTitleCase(input.Trim().ToLowerInvariant());
    return Regex.Replace(cleaned, @"\s+", " ");
  }
  public static string? NormalizeDescripcionEstado(string? input) => string.IsNullOrWhiteSpace(input) ? null : Regex.Replace(input.Trim(), @"\s+", " ").ToUpperInvariant();

  // --- NOMBRE (usuarios, sectores, etc.) ---
  public static string? NormalizeNombre(string? input) => string.IsNullOrWhiteSpace(input) ? null : Regex.Replace(input.Trim(), @"\s+", " ").ToUpperInvariant();

  // --- TIPO 
  public static string? NormalizeTipoRol(string? input)
  {
    if (string.IsNullOrWhiteSpace(input)) return null;
    var textInfo = CultureInfo.InvariantCulture.TextInfo;
    var cleaned = textInfo.ToTitleCase(input.Trim().ToLowerInvariant());
    return Regex.Replace(cleaned, @"\s+", " ");
  }
  public static string? NormalizeTipoMostrador(string? input)
  {
    return string.IsNullOrWhiteSpace(input) ? null : Regex.Replace(input.Trim(), @"\s+", " ").ToUpperInvariant();
  }

  // --- TITULAR (clientes) ---
  public static string? NormalizeTitular(string? input)
  {
    if (string.IsNullOrWhiteSpace(input)) return null;
    var textInfo = CultureInfo.InvariantCulture.TextInfo;
    var cleaned = textInfo.ToTitleCase(input.Trim().ToLowerInvariant());
    return Regex.Replace(cleaned, @"\s+", " ");
  }

  // --- USERNAME (usuarios) ---
  public static string? NormalizeUsername(string? input) => string.IsNullOrWhiteSpace(input) ? null : input.Trim().ToLowerInvariant();

  // --- DNI (clientes) ---
  public static string? NormalizeDni(string? input) => string.IsNullOrWhiteSpace(input) ? null : Regex.Replace(input.Trim(), @"\D", "");
}