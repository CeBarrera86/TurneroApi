export class PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;

  constructor(items: T[], total: number, page: number, pageSize: number) {
    this.items = items;
    this.total = total;
    this.page = page;
    this.pageSize = pageSize;
    this.totalPages = Math.ceil(total / pageSize);
  }
}

export class PaginationHelper {
  static isValid(page: number, pageSize: number): { valid: boolean; message?: string } {
    if (page < 1) {
      return { valid: false, message: 'El número de página debe ser mayor o igual a 1.' };
    }

    if (pageSize < 1 || pageSize > 100) {
      return { valid: false, message: 'El tamaño de página debe estar entre 1 y 100.' };
    }

    return { valid: true };
  }

  static async paginate<T>(query: any, page: number, pageSize: number): Promise<PagedResult<T>> {
    const skip = (page - 1) * pageSize;
    const [items, total] = await query.skip(skip).take(pageSize).getManyAndCount();

    return new PagedResult<T>(items, total, page, pageSize);
  }
}
