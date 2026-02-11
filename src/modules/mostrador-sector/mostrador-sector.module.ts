import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { MostradorSector } from '../../entities/mostrador-sector.entity';

@Module({
  imports: [TypeOrmModule.forFeature([MostradorSector])],
  controllers: [],
  providers: [],
  exports: [],
})
export class MostradorSectorModule {}
