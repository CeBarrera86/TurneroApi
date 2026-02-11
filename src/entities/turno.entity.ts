import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, OneToMany, JoinColumn } from 'typeorm';
import { Puesto } from './puesto.entity';
import { Ticket } from './ticket.entity';
import { Estado } from './estado.entity';
import { Historial } from './historial.entity';

@Entity('turnos')
export class Turno {
  @PrimaryGeneratedColumn({ type: 'bigint', unsigned: true })
  id: string;

  @Column({ name: 'puesto_id' })
  puestoId: number;

  @Column({ name: 'ticket_id', type: 'bigint', unsigned: true })
  ticketId: string;

  @Column({ name: 'fecha_inicio', type: 'datetime' })
  fechaInicio: Date;

  @Column({ name: 'fecha_fin', type: 'datetime', nullable: true })
  fechaFin: Date;

  @Column({ name: 'estado_id' })
  estadoId: number;

  @ManyToOne(() => Puesto, (puesto) => puesto.turnos)
  @JoinColumn({ name: 'puesto_id' })
  puestoNavigation: Puesto;

  @ManyToOne(() => Ticket, (ticket) => ticket.turnos)
  @JoinColumn({ name: 'ticket_id' })
  ticketNavigation: Ticket;

  @ManyToOne(() => Estado, (estado) => estado.turnos)
  @JoinColumn({ name: 'estado_id' })
  estadoNavigation: Estado;

  @OneToMany(() => Historial, (historial) => historial.turnoNavigation)
  historiales: Historial[];
}
