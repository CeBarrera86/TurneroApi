import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  CreateDateColumn,
  UpdateDateColumn,
  OneToMany,
} from 'typeorm';
import { Ticket } from './ticket.entity';
import { Turno } from './turno.entity';
import { Historial } from './historial.entity';

@Entity('estados')
export class Estado {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ type: 'char', length: 1 })
  letra: string;

  @Column({ type: 'varchar', length: 50 })
  descripcion: string;

  @CreateDateColumn({ name: 'created_at' })
  createdAt: Date;

  @UpdateDateColumn({ name: 'updated_at' })
  updatedAt: Date;

  @OneToMany(() => Ticket, (ticket) => ticket.estadoNavigation)
  tickets: Ticket[];

  @OneToMany(() => Turno, (turno) => turno.estadoNavigation)
  turnos: Turno[];

  @OneToMany(() => Historial, (historial) => historial.estadoNavigation)
  historiales: Historial[];
}
