import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Historial } from '../../entities/historial.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Historial])],
  controllers: [],
  providers: [],
  exports: [],
})
export class HistorialModule {}
