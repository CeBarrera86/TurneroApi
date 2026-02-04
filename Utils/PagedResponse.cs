namespace TurneroApi.Utils;

public sealed class PagedResponse<T>
{
  public PagedResponse(IReadOnlyList<T> data, int page, int pageSize, int total)
  {
    Data = data;
    Page = page;
    PageSize = pageSize;
    Total = total;
  }

  public IReadOnlyList<T> Data { get; }
  public int Page { get; }
  public int PageSize { get; }
  public int Total { get; }
}
