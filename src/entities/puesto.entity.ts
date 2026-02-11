import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, OneToMany, JoinColumn } from 'typeorm';
import { Usuario } from './usuario.entity';
import { Mostrador } from './mostrador.entity';
import { Turno } from './turno.entity';
import { Historial } from './historial.entity';

@Entity('puestos')
export class Puesto {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ name: 'usuario_id' })
  usuarioId: number;

  @Column({ name: 'mostrador_id' })
  mostradorId: number;

  @Column({ type: 'datetime', nullable: true })
  login: Date;

  @Column({ type: 'datetime', nullable: true })
  logout: Date;

  @Column({ type: 'boolean', default: false })
  activo: boolean;

  @ManyToOne(() => Usuario, (usuario) => usuario.puestos)
  @JoinColumn({ name: 'usuario_id' })
  usuarioNavigation: Usuario;

  @ManyToOne(() => Mostrador, (mostrador) => mostrador.puestos)
  @JoinColumn({ name: 'mostrador_id' })
  mostradorNavigation: Mostrador;

  @OneToMany(() => Turno, (turno) => turno.puestoNavigation)
  turnos: Turno[];

  @OneToMany(() => Historial, (historial) => historial.puestoNavigation)
  historiales: Historial[];
}
