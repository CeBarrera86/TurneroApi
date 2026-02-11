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
  NotFoundException,
} from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth, ApiResponse } from '@nestjs/swagger';
import { TurnoService } from './turno.service';
import { TurnoCrearDto, TurnoActualizarDto, TurnoDto } from '../../dto/turno.dto';
import { JwtAuthGuard } from '../../guards/jwt-auth.guard';
import { PermissionsGuard } from '../../guards/permissions.guard';
import { RequirePermissions } from '../../decorators/permissions.decorator';
import { ApiPagination } from '../../decorators/api-pagination.decorator';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@ApiTags('Turnos')
@ApiBearerAuth('JWT-auth')
@Controller('turno')
@UseGuards(JwtAuthGuard, PermissionsGuard)
export class TurnoController {
  constructor(private readonly turnoService: TurnoService) {}

  @Get()
  @RequirePermissions('ver_turno')
  @ApiOperation({ summary: 'Obtener todos los turnos paginados' })
  @ApiPagination()
  @ApiResponse({ status: 200, description: 'Lista de turnos' })
  async findAll(
    @Query('page') page: number = 1,
    @Query('pageSize') pageSize: number = 10,
  ): Promise<PagedResult<TurnoDto>> {
    const validation = PaginationHelper.isValid(page, pageSize);
    if (!validation.valid) {
      throw new BadRequestException(validation.message);
    }

    return await this.turnoService.findAll(page, pageSize);
  }

  @Get(':id')
  @RequirePermissions('ver_turno')
  @ApiOperation({ summary: 'Obtener un turno por ID' })
  @ApiResponse({ status: 200, description: 'Turno encontrado' })
  @ApiResponse({ status: 404, description: 'Turno no encontrado' })
  async findOne(@Param('id') id: string): Promise<TurnoDto> {
    return await this.turnoService.findOne(id);
  }

  @Get('puesto/:puestoId/activo')
  @RequirePermissions('ver_turno')
  @ApiOperation({ summary: 'Obtener turno activo por puesto' })
  @ApiResponse({ status: 200, description: 'Turno activo encontrado' })
  @ApiResponse({ status: 404, description: 'No se encontró turno activo' })
  async findTurnoActivoPorPuesto(@Param('puestoId') puestoId: number): Promise<TurnoDto> {
    const turno = await this.turnoService.findTurnoActivoPorPuestoId(puestoId);
    if (!turno) {
      throw new NotFoundException(
        `No se encontró ningún turno activo para el puesto '${puestoId}'.`,
      );
    }
    return turno;
  }

  @Post()
  @RequirePermissions('crear_turno')
  @ApiOperation({ summary: 'Crear un nuevo turno' })
  @ApiResponse({ status: 201, description: 'Turno creado exitosamente' })
  @ApiResponse({ status: 400, description: 'Datos inválidos' })
  async create(@Body() turnoCrearDto: TurnoCrearDto): Promise<TurnoDto> {
    const { turno, errorMessage } = await this.turnoService.create(turnoCrearDto);

    if (!turno) {
      throw new BadRequestException(errorMessage);
    }

    return turno as any;
  }

  @Put(':id')
  @RequirePermissions('editar_turno')
  @ApiOperation({ summary: 'Actualizar un turno' })
  @ApiResponse({ status: 200, description: 'Turno actualizado exitosamente' })
  @ApiResponse({ status: 404, description: 'Turno no encontrado' })
  async update(
    @Param('id') id: string,
    @Body() turnoActualizarDto: TurnoActualizarDto,
  ): Promise<TurnoDto> {
    return (await this.turnoService.update(id, turnoActualizarDto)) as any;
  }

  @Delete(':id')
  @RequirePermissions('eliminar_turno')
  @ApiOperation({ summary: 'Eliminar un turno' })
  @ApiResponse({ status: 200, description: 'Turno eliminado exitosamente' })
  @ApiResponse({ status: 404, description: 'Turno no encontrado' })
  async remove(@Param('id') id: string): Promise<{ message: string }> {
    await this.turnoService.remove(id);
    return { message: 'Turno eliminado exitosamente' };
  }
}
