import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, JoinColumn } from 'typeorm';
import { Ticket } from './ticket.entity';
import { Estado } from './estado.entity';
import { Puesto } from './puesto.entity';
import { Turno } from './turno.entity';
import { Usuario } from './usuario.entity';

@Entity('historiales')
export class Historial {
  @PrimaryGeneratedColumn({ type: 'bigint', unsigned: true })
  id: string;

  @Column({ name: 'ticket_id', type: 'bigint', unsigned: true })
  ticketId: string;

  @Column({ name: 'estado_id' })
  estadoId: number;

  @Column({ type: 'datetime' })
  fecha: Date;

  @Column({ name: 'puesto_id', nullable: true })
  puestoId: number;

  @Column({ name: 'turno_id', type: 'bigint', unsigned: true, nullable: true })
  turnoId: string;

  @Column({ name: 'usuario_id', nullable: true })
  usuarioId: number;

  @Column({ type: 'text', nullable: true })
  comentarios: string;

  @ManyToOne(() => Ticket, (ticket) => ticket.historiales)
  @JoinColumn({ name: 'ticket_id' })
  ticketNavigation: Ticket;

  @ManyToOne(() => Estado, (estado) => estado.historiales)
  @JoinColumn({ name: 'estado_id' })
  estadoNavigation: Estado;

  @ManyToOne(() => Puesto, (puesto) => puesto.historiales, { nullable: true })
  @JoinColumn({ name: 'puesto_id' })
  puestoNavigation: Puesto;

  @ManyToOne(() => Turno, (turno) => turno.historiales, { nullable: true })
  @JoinColumn({ name: 'turno_id' })
  turnoNavigation: Turno;

  @ManyToOne(() => Usuario, (usuario) => usuario.historiales, { nullable: true })
  @JoinColumn({ name: 'usuario_id' })
  usuarioNavigation: Usuario;
}
