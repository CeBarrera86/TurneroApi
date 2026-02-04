namespace TurneroApi.Utils;

public static class PaginationHelper
{
  public const int MaxPageSize = 100;
  public const string InvalidPaginationMessage = "Parámetros de paginación inválidos. 'page' debe ser > 0 y 'pageSize' entre 1 y 100.";

  public static bool IsValid(int page, int pageSize, out string message)
  {
    if (page <= 0 || pageSize <= 0 || pageSize > MaxPageSize)
    {
      message = InvalidPaginationMessage;
      return false;
    }

    message = string.Empty;
    return true;
  }
}
