import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { RolPermiso } from '../../entities/rol-permiso.entity';

@Module({
  imports: [TypeOrmModule.forFeature([RolPermiso])],
  controllers: [],
  providers: [],
  exports: [],
})
export class RolPermisoModule {}
