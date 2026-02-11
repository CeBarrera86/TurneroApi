import {
  Controller,
  Get,
  Post,
  Put,
  Delete,
  Body,
  Param,
  Query,
  UseGuards,
  BadRequestException,
} from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth } from '@nestjs/swagger';
import { EstadoService } from './estado.service';
import { EstadoCrearDto, EstadoActualizarDto, EstadoDto } from '../../dto/estado.dto';
import { JwtAuthGuard } from '../../guards/jwt-auth.guard';
import { PermissionsGuard } from '../../guards/permissions.guard';
import { RequirePermissions } from '../../decorators/permissions.decorator';
import { ApiPagination } from '../../decorators/api-pagination.decorator';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@ApiTags('Estados')
@ApiBearerAuth('JWT-auth')
@Controller('estado')
@UseGuards(JwtAuthGuard, PermissionsGuard)
export class EstadoController {
  constructor(private readonly estadoService: EstadoService) {}

  @Get()
  @RequirePermissions('ver_estado')
  @ApiOperation({ summary: 'Obtener todos los estados paginados' })
  @ApiPagination()
  async findAll(
    @Query('page') page: number = 1,
    @Query('pageSize') pageSize: number = 10,
  ): Promise<PagedResult<EstadoDto>> {
    const validation = PaginationHelper.isValid(page, pageSize);
    if (!validation.valid) throw new BadRequestException(validation.message);
    return await this.estadoService.findAll(page, pageSize);
  }

  @Get(':id')
  @RequirePermissions('ver_estado')
  @ApiOperation({ summary: 'Obtener un estado por ID' })
  async findOne(@Param('id') id: number): Promise<EstadoDto> {
    return await this.estadoService.findOne(id);
  }

  @Post()
  @RequirePermissions('crear_estado')
  @ApiOperation({ summary: 'Crear un nuevo estado' })
  async create(@Body() estadoCrearDto: EstadoCrearDto): Promise<EstadoDto> {
    return (await this.estadoService.create(estadoCrearDto)) as any;
  }

  @Put(':id')
  @RequirePermissions('editar_estado')
  @ApiOperation({ summary: 'Actualizar un estado' })
  async update(
    @Param('id') id: number,
    @Body() estadoActualizarDto: EstadoActualizarDto,
  ): Promise<EstadoDto> {
    return (await this.estadoService.update(id, estadoActualizarDto)) as any;
  }

  @Delete(':id')
  @RequirePermissions('eliminar_estado')
  @ApiOperation({ summary: 'Eliminar un estado' })
  async remove(@Param('id') id: number): Promise<{ message: string }> {
    await this.estadoService.remove(id);
    return { message: 'Estado eliminado exitosamente' };
  }
}
