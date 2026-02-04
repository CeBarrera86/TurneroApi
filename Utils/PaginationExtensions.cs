using Microsoft.EntityFrameworkCore;

namespace TurneroApi.Utils;

public static class PaginationExtensions
{
  public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
  {
    var total = await query.CountAsync(cancellationToken);
    var items = await query
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync(cancellationToken);

    return new PagedResult<T>(items, total);
  }
}
