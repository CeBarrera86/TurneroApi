import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Puesto } from '../../entities/puesto.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Puesto])],
  controllers: [],
  providers: [],
  exports: [],
})
export class PuestoModule {}
