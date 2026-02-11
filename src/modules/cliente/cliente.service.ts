import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Cliente } from '../../entities/cliente.entity';
import { ClienteCrearDto, ClienteActualizarDto, ClienteDto } from '../../dto/cliente.dto';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@Injectable()
export class ClienteService {
  constructor(
    @InjectRepository(Cliente)
    private clienteRepository: Repository<Cliente>,
  ) {}

  async findAll(page: number = 1, pageSize: number = 10): Promise<PagedResult<ClienteDto>> {
    const query = this.clienteRepository
      .createQueryBuilder('cliente')
      .orderBy('cliente.id', 'DESC');
    return await PaginationHelper.paginate<ClienteDto>(query, page, pageSize);
  }

  async findOne(id: string): Promise<ClienteDto> {
    const cliente = await this.clienteRepository.findOne({ where: { id } });
    if (!cliente) {
      throw new NotFoundException(`Cliente con ID ${id} no encontrado`);
    }
    return cliente as any;
  }

  async findByDni(dni: string): Promise<ClienteDto | null> {
    const cliente = await this.clienteRepository.findOne({ where: { dni } });
    return cliente as any;
  }

  async create(clienteCrearDto: ClienteCrearDto): Promise<Cliente> {
    const cliente = this.clienteRepository.create(clienteCrearDto);
    return await this.clienteRepository.save(cliente);
  }

  async update(id: string, clienteActualizarDto: ClienteActualizarDto): Promise<Cliente> {
    await this.findOne(id);
    await this.clienteRepository.update(id, clienteActualizarDto);
    return (await this.findOne(id)) as any;
  }

  async remove(id: string): Promise<void> {
    const cliente = await this.findOne(id);
    await this.clienteRepository.remove(cliente as any);
  }
}
