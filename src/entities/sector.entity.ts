import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  CreateDateColumn,
  UpdateDateColumn,
  OneToMany,
} from 'typeorm';
import { Ticket } from './ticket.entity';
import { MostradorSector } from './mostrador-sector.entity';

@Entity('sectores')
export class Sector {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ type: 'varchar', length: 100 })
  nombre: string;

  @Column({ type: 'text', nullable: true })
  descripcion: string;

  @Column({ type: 'boolean', default: true })
  activo: boolean;

  @CreateDateColumn({ name: 'created_at' })
  createdAt: Date;

  @UpdateDateColumn({ name: 'updated_at' })
  updatedAt: Date;

  @OneToMany(() => Ticket, (ticket) => ticket.sectorIdOrigenNavigation)
  ticketsOrigen: Ticket[];

  @OneToMany(() => Ticket, (ticket) => ticket.sectorIdActualNavigation)
  ticketsActual: Ticket[];

  @OneToMany(() => MostradorSector, (mostradorSector) => mostradorSector.sectorNavigation)
  mostradorSectores: MostradorSector[];
}
