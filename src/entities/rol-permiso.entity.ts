import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  ManyToOne,
  JoinColumn,
  CreateDateColumn,
} from 'typeorm';
import { Rol } from './rol.entity';
import { Permiso } from './permiso.entity';

@Entity('rol_permisos')
export class RolPermiso {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ name: 'rol_id' })
  rolId: number;

  @Column({ name: 'permiso_id' })
  permisoId: number;

  @CreateDateColumn({ name: 'created_at' })
  createdAt: Date;

  @ManyToOne(() => Rol, (rol) => rol.rolPermisos)
  @JoinColumn({ name: 'rol_id' })
  rolNavigation: Rol;

  @ManyToOne(() => Permiso, (permiso) => permiso.rolPermisos)
  @JoinColumn({ name: 'permiso_id' })
  permisoNavigation: Permiso;
}
