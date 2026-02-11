import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Estado } from '../../entities/estado.entity';
import { EstadoCrearDto, EstadoActualizarDto, EstadoDto } from '../../dto/estado.dto';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@Injectable()
export class EstadoService {
  constructor(
    @InjectRepository(Estado)
    private estadoRepository: Repository<Estado>,
  ) {}

  async findAll(page: number = 1, pageSize: number = 10): Promise<PagedResult<EstadoDto>> {
    const query = this.estadoRepository.createQueryBuilder('estado').orderBy('estado.id', 'ASC');
    return await PaginationHelper.paginate<EstadoDto>(query, page, pageSize);
  }

  async findOne(id: number): Promise<EstadoDto> {
    const estado = await this.estadoRepository.findOne({ where: { id } });
    if (!estado) {
      throw new NotFoundException(`Estado con ID ${id} no encontrado`);
    }
    return estado as any;
  }

  async create(estadoCrearDto: EstadoCrearDto): Promise<Estado> {
    const estado = this.estadoRepository.create(estadoCrearDto);
    return await this.estadoRepository.save(estado);
  }

  async update(id: number, estadoActualizarDto: EstadoActualizarDto): Promise<Estado> {
    await this.findOne(id);
    await this.estadoRepository.update(id, estadoActualizarDto);
    return (await this.findOne(id)) as any;
  }

  async remove(id: number): Promise<void> {
    const estado = await this.findOne(id);
    await this.estadoRepository.remove(estado as any);
  }
}
