import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';
import { typeOrmConfig } from './config/typeorm.config';
import { AuthModule } from './modules/auth/auth.module';
import { ClienteModule } from './modules/cliente/cliente.module';
import { ContenidoModule } from './modules/contenido/contenido.module';
import { EstadoModule } from './modules/estado/estado.module';
import { HistorialModule } from './modules/historial/historial.module';
import { MostradorModule } from './modules/mostrador/mostrador.module';
import { MostradorSectorModule } from './modules/mostrador-sector/mostrador-sector.module';
import { PermisoModule } from './modules/permiso/permiso.module';
import { PuestoModule } from './modules/puesto/puesto.module';
import { RolModule } from './modules/rol/rol.module';
import { RolPermisoModule } from './modules/rol-permiso/rol-permiso.module';
import { SectorModule } from './modules/sector/sector.module';
import { TicketModule } from './modules/ticket/ticket.module';
import { TurnoModule } from './modules/turno/turno.module';
import { UsuarioModule } from './modules/usuario/usuario.module';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
      envFilePath: '.env',
    }),
    TypeOrmModule.forRootAsync({
      useFactory: () => typeOrmConfig,
    }),
    AuthModule,
    ClienteModule,
    ContenidoModule,
    EstadoModule,
    HistorialModule,
    MostradorModule,
    MostradorSectorModule,
    PermisoModule,
    PuestoModule,
    RolModule,
    RolPermisoModule,
    SectorModule,
    TicketModule,
    TurnoModule,
    UsuarioModule,
  ],
  controllers: [],
  providers: [],
})
export class AppModule {}
