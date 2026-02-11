import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Contenido } from '../../entities/contenido.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Contenido])],
  controllers: [],
  providers: [],
  exports: [],
})
export class ContenidoModule {}
