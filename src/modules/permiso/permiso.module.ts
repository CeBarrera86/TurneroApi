import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Permiso } from '../../entities/permiso.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Permiso])],
  controllers: [],
  providers: [],
  exports: [],
})
export class PermisoModule {}
