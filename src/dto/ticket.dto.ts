import { IsNotEmpty, IsString, IsNumber, IsBoolean, IsOptional } from 'class-validator';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';

export class TicketCrearDto {
  @ApiProperty({ example: 'A' })
  @IsNotEmpty({ message: 'La letra es obligatoria.' })
  @IsString({ message: 'La letra debe ser un string.' })
  letra: string;

  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El número es obligatorio.' })
  @IsNumber({}, { message: 'El número debe ser un número.' })
  numero: number;

  @ApiProperty({ example: '1' })
  @IsNotEmpty({ message: 'El cliente es obligatorio.' })
  @IsString({ message: 'El cliente debe ser un string.' })
  clienteId: string;

  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El sector de origen es obligatorio.' })
  @IsNumber({}, { message: 'El sector de origen debe ser un número.' })
  sectorIdOrigen: number;

  @ApiPropertyOptional({ example: 1 })
  @IsOptional()
  @IsNumber({}, { message: 'El sector actual debe ser un número.' })
  sectorIdActual?: number;

  @ApiProperty({ example: 4 })
  @IsNotEmpty({ message: 'El estado es obligatorio.' })
  @IsNumber({}, { message: 'El estado debe ser un número.' })
  estadoId: number;
}

export class TicketActualizarDto {
  @ApiPropertyOptional()
  @IsOptional()
  @IsNumber({}, { message: 'El sector actual debe ser un número.' })
  sectorIdActual?: number;

  @ApiPropertyOptional()
  @IsOptional()
  @IsNumber({}, { message: 'El estado debe ser un número.' })
  estadoId?: number;
}

export class TicketDto {
  @ApiProperty()
  id: string;

  @ApiProperty()
  letra: string;

  @ApiProperty()
  numero: number;

  @ApiProperty()
  clienteId: string;

  @ApiProperty()
  fecha: Date;

  @ApiProperty()
  sectorIdOrigen: number;

  @ApiPropertyOptional()
  sectorIdActual?: number;

  @ApiProperty()
  estadoId: number;

  @ApiPropertyOptional()
  actualizado?: Date;
}
