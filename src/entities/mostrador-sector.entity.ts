import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, JoinColumn } from 'typeorm';
import { Mostrador } from './mostrador.entity';
import { Sector } from './sector.entity';

@Entity('mostrador_sectores')
export class MostradorSector {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ name: 'mostrador_id' })
  mostradorId: number;

  @Column({ name: 'sector_id' })
  sectorId: number;

  @ManyToOne(() => Mostrador, (mostrador) => mostrador.mostradorSectores)
  @JoinColumn({ name: 'mostrador_id' })
  mostradorNavigation: Mostrador;

  @ManyToOne(() => Sector, (sector) => sector.mostradorSectores)
  @JoinColumn({ name: 'sector_id' })
  sectorNavigation: Sector;
}
