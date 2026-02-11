import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Mostrador } from '../../entities/mostrador.entity';

@Module({
  imports: [TypeOrmModule.forFeature([Mostrador])],
  controllers: [],
  providers: [],
  exports: [],
})
export class MostradorModule {}
