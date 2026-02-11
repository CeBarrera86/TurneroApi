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
import { ClienteService } from './cliente.service';
import { ClienteCrearDto, ClienteActualizarDto, ClienteDto } from '../../dto/cliente.dto';
import { JwtAuthGuard } from '../../guards/jwt-auth.guard';
import { PermissionsGuard } from '../../guards/permissions.guard';
import { RequirePermissions } from '../../decorators/permissions.decorator';
import { ApiPagination } from '../../decorators/api-pagination.decorator';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@ApiTags('Clientes')
@ApiBearerAuth('JWT-auth')
@Controller('cliente')
@UseGuards(JwtAuthGuard, PermissionsGuard)
export class ClienteController {
  constructor(private readonly clienteService: ClienteService) {}

  @Get()
  @RequirePermissions('ver_cliente')
  @ApiOperation({ summary: 'Obtener todos los clientes paginados' })
  @ApiPagination()
  async findAll(
    @Query('page') page: number = 1,
    @Query('pageSize') pageSize: number = 10,
  ): Promise<PagedResult<ClienteDto>> {
    const validation = PaginationHelper.isValid(page, pageSize);
    if (!validation.valid) {
      throw new BadRequestException(validation.message);
    }
    return await this.clienteService.findAll(page, pageSize);
  }

  @Get(':id')
  @RequirePermissions('ver_cliente')
  @ApiOperation({ summary: 'Obtener un cliente por ID' })
  async findOne(@Param('id') id: string): Promise<ClienteDto> {
    return await this.clienteService.findOne(id);
  }

  @Post()
  @RequirePermissions('crear_cliente')
  @ApiOperation({ summary: 'Crear un nuevo cliente' })
  async create(@Body() clienteCrearDto: ClienteCrearDto): Promise<ClienteDto> {
    return (await this.clienteService.create(clienteCrearDto)) as any;
  }

  @Put(':id')
  @RequirePermissions('editar_cliente')
  @ApiOperation({ summary: 'Actualizar un cliente' })
  async update(
    @Param('id') id: string,
    @Body() clienteActualizarDto: ClienteActualizarDto,
  ): Promise<ClienteDto> {
    return (await this.clienteService.update(id, clienteActualizarDto)) as any;
  }

  @Delete(':id')
  @RequirePermissions('eliminar_cliente')
  @ApiOperation({ summary: 'Eliminar un cliente' })
  async remove(@Param('id') id: string): Promise<{ message: string }> {
    await this.clienteService.remove(id);
    return { message: 'Cliente eliminado exitosamente' };
  }
}
