namespace TurneroApi.Utils;

public sealed class PagedResult<T>
{
  public PagedResult(IReadOnlyList<T> items, int total)
  {
    Items = items;
    Total = total;
  }

  public IReadOnlyList<T> Items { get; }
  public int Total { get; }
}
