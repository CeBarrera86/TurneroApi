import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { TurnoController } from './turno.controller';
import { TurnoService } from './turno.service';
import { Turno } from '../../entities/turno.entity';
import { Estado } from '../../entities/estado.entity';
import { Puesto } from '../../entities/puesto.entity';
import { Ticket } from '../../entities/ticket.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Turno, Estado, Puesto, Ticket])],
  controllers: [TurnoController],
  providers: [TurnoService],
  exports: [TurnoService],
})
export class TurnoModule {}
