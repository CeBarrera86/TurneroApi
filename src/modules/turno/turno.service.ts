import { Injectable, NotFoundException, BadRequestException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Turno } from '../../entities/turno.entity';
import { Estado } from '../../entities/estado.entity';
import { Puesto } from '../../entities/puesto.entity';
import { Ticket } from '../../entities/ticket.entity';
import { TurnoCrearDto, TurnoActualizarDto, TurnoDto } from '../../dto/turno.dto';
import { PaginationHelper, PagedResult } from '../../utils/pagination.helper';

@Injectable()
export class TurnoService {
  constructor(
    @InjectRepository(Turno)
    private turnoRepository: Repository<Turno>,
    @InjectRepository(Estado)
    private estadoRepository: Repository<Estado>,
    @InjectRepository(Puesto)
    private puestoRepository: Repository<Puesto>,
    @InjectRepository(Ticket)
    private ticketRepository: Repository<Ticket>,
  ) {}

  async findAll(page: number = 1, pageSize: number = 10): Promise<PagedResult<TurnoDto>> {
    const query = this.turnoRepository
      .createQueryBuilder('turno')
      .leftJoinAndSelect('turno.puestoNavigation', 'puesto')
      .leftJoinAndSelect('turno.ticketNavigation', 'ticket')
      .leftJoinAndSelect('turno.estadoNavigation', 'estado')
      .orderBy('turno.fechaInicio', 'DESC');

    return await PaginationHelper.paginate<TurnoDto>(query, page, pageSize);
  }

  async findOne(id: string): Promise<TurnoDto> {
    const turno = await this.turnoRepository.findOne({
      where: { id },
      relations: ['puestoNavigation', 'ticketNavigation', 'estadoNavigation'],
    });

    if (!turno) {
      throw new NotFoundException(`Turno con ID ${id} no encontrado`);
    }

    return turno as any;
  }

  async findTurnoActivoPorPuestoId(puestoId: number): Promise<TurnoDto | null> {
    const estadoAtendido = await this.estadoRepository.findOne({
      where: { id: 1, descripcion: 'ATENDIDO' },
    });

    if (!estadoAtendido) {
      throw new BadRequestException(
        'El estado ATENDIDO (ID 1) no está configurado en la base de datos.',
      );
    }

    const turno = await this.turnoRepository.findOne({
      where: { puestoId, estadoId: estadoAtendido.id },
      relations: ['puestoNavigation', 'ticketNavigation', 'estadoNavigation'],
    });

    return turno as any;
  }

  async create(turnoCrearDto: TurnoCrearDto): Promise<{ turno: Turno | null; errorMessage?: string }> {
    // Validaciones de existencia
    const puestoExiste = await this.puestoRepository.exists({
      where: { id: turnoCrearDto.puestoId },
    });
    if (!puestoExiste) {
      return {
        turno: null,
        errorMessage: `El PuestoId '${turnoCrearDto.puestoId}' no existe.`,
      };
    }

    const ticketExiste = await this.ticketRepository.exists({
      where: { id: turnoCrearDto.ticketId },
    });
    if (!ticketExiste) {
      return {
        turno: null,
        errorMessage: `El TicketId '${turnoCrearDto.ticketId}' no existe.`,
      };
    }

    // Estado ATENDIDO (ID = 1)
    const estadoAtendido = await this.estadoRepository.findOne({
      where: { id: 1, descripcion: 'ATENDIDO' },
    });
    if (!estadoAtendido) {
      return {
        turno: null,
        errorMessage: "El estado 'ATENDIDO' (ID 1) no está configurado en la base de datos.",
      };
    }

    // Un puesto no puede tener más de un turno atendido activo
    const turnoActivoExistente = await this.turnoRepository.exists({
      where: { puestoId: turnoCrearDto.puestoId, estadoId: estadoAtendido.id },
    });
    if (turnoActivoExistente) {
      return {
        turno: null,
        errorMessage: `El puesto '${turnoCrearDto.puestoId}' ya tiene un turno 'ATENDIDO' activo.`,
      };
    }

    // Un ticket no puede estar atendido en múltiples turnos simultáneamente
    const ticketAtendidoExistente = await this.turnoRepository.exists({
      where: { ticketId: turnoCrearDto.ticketId, estadoId: estadoAtendido.id },
    });
    if (ticketAtendidoExistente) {
      return {
        turno: null,
        errorMessage: `El Ticket '${turnoCrearDto.ticketId}' ya está siendo atendido en otro turno.`,
      };
    }

    const turno = this.turnoRepository.create({
      ...turnoCrearDto,
      fechaInicio: new Date(),
      estadoId: estadoAtendido.id,
      fechaFin: undefined,
    });

    try {
      const savedTurno = await this.turnoRepository.save(turno);
      return { turno: savedTurno };
    } catch (error) {
      return {
        turno: null,
        errorMessage: 'Error al crear el turno: ' + error.message,
      };
    }
  }

  async update(id: string, turnoActualizarDto: TurnoActualizarDto): Promise<Turno> {
    const turno = await this.findOne(id);

    if (turnoActualizarDto.estadoId !== undefined) {
      const estadoExiste = await this.estadoRepository.exists({
        where: { id: turnoActualizarDto.estadoId },
      });
      if (!estadoExiste) {
        throw new BadRequestException(`El EstadoId '${turnoActualizarDto.estadoId}' no existe.`);
      }
    }

    Object.assign(turno, turnoActualizarDto);
    return await this.turnoRepository.save(turno as any);
  }

  async remove(id: string): Promise<void> {
    const turno = await this.findOne(id);
    await this.turnoRepository.remove(turno as any);
  }
}
