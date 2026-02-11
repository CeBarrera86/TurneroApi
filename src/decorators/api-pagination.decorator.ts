import { applyDecorators } from '@nestjs/common';
import { ApiQuery } from '@nestjs/swagger';

export function ApiPagination() {
  return applyDecorators(
    ApiQuery({
      name: 'page',
      required: false,
      type: Number,
      example: 1,
      description: 'Número de página (por defecto: 1)',
    }),
    ApiQuery({
      name: 'pageSize',
      required: false,
      type: Number,
      example: 10,
      description: 'Cantidad de elementos por página (por defecto: 10, máximo: 100)',
    }),
  );
}
