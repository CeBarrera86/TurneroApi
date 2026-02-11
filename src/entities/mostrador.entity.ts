import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  CreateDateColumn,
  UpdateDateColumn,
  OneToMany,
} from 'typeorm';
import { Puesto } from './puesto.entity';
import { MostradorSector } from './mostrador-sector.entity';

@Entity('mostradores')
export class Mostrador {
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

  @OneToMany(() => Puesto, (puesto) => puesto.mostradorNavigation)
  puestos: Puesto[];

  @OneToMany(() => MostradorSector, (mostradorSector) => mostradorSector.mostradorNavigation)
  mostradorSectores: MostradorSector[];
}
