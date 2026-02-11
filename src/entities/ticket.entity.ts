import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  ManyToOne,
  OneToMany,
  JoinColumn,
  CreateDateColumn,
  UpdateDateColumn,
} from 'typeorm';
import { Cliente } from './cliente.entity';
import { Sector } from './sector.entity';
import { Estado } from './estado.entity';
import { Turno } from './turno.entity';
import { Historial } from './historial.entity';

@Entity('tickets')
export class Ticket {
  @PrimaryGeneratedColumn({ type: 'bigint', unsigned: true })
  id: string;

  @Column({ type: 'char', length: 1 })
  letra: string;

  @Column({ type: 'int' })
  numero: number;

  @Column({ name: 'cliente_id', type: 'bigint', unsigned: true })
  clienteId: string;

  @Column({ type: 'datetime' })
  fecha: Date;

  @Column({ name: 'sector_id_origen' })
  sectorIdOrigen: number;

  @Column({ name: 'sector_id_actual', nullable: true })
  sectorIdActual: number;

  @Column({ name: 'estado_id' })
  estadoId: number;

  @UpdateDateColumn({ name: 'actualizado', nullable: true })
  actualizado: Date;

  @ManyToOne(() => Cliente, (cliente) => cliente.tickets)
  @JoinColumn({ name: 'cliente_id' })
  clienteNavigation: Cliente;

  @ManyToOne(() => Sector, (sector) => sector.ticketsOrigen)
  @JoinColumn({ name: 'sector_id_origen' })
  sectorIdOrigenNavigation: Sector;

  @ManyToOne(() => Sector, (sector) => sector.ticketsActual)
  @JoinColumn({ name: 'sector_id_actual' })
  sectorIdActualNavigation: Sector;

  @ManyToOne(() => Estado, (estado) => estado.tickets)
  @JoinColumn({ name: 'estado_id' })
  estadoNavigation: Estado;

  @OneToMany(() => Turno, (turno) => turno.ticketNavigation)
  turnos: Turno[];

  @OneToMany(() => Historial, (historial) => historial.ticketNavigation)
  historiales: Historial[];
}
