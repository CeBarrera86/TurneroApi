import { Entity, PrimaryGeneratedColumn, Column, OneToMany } from 'typeorm';
import { Ticket } from './ticket.entity';

@Entity('clientes')
export class Cliente {
  @PrimaryGeneratedColumn({ type: 'bigint', unsigned: true })
  id: string;

  @Column({ type: 'varchar', length: 20, unique: true })
  dni: string;

  @Column({ type: 'varchar', length: 255 })
  titular: string;

  @OneToMany(() => Ticket, (ticket) => ticket.clienteNavigation)
  tickets: Ticket[];
}
