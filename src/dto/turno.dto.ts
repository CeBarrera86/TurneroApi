import { IsNotEmpty, IsNumber, IsDate, IsOptional, IsString } from 'class-validator';
import { Type } from 'class-transformer';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';

export class TurnoCrearDto {
  @ApiProperty({ description: 'ID del puesto', example: 1 })
  @IsNotEmpty({ message: 'El puesto es obligatorio.' })
  @IsNumber({}, { message: 'El puesto debe ser un número.' })
  puestoId: number;

  @ApiProperty({ description: 'ID del ticket', example: '1' })
  @IsNotEmpty({ message: 'El ticket es obligatorio.' })
  @IsString({ message: 'El ticket debe ser un string.' })
  ticketId: string;
}

export class TurnoActualizarDto {
  @ApiPropertyOptional({ description: 'Fecha de fin del turno' })
  @IsOptional()
  @IsDate({ message: 'La fecha de fin debe ser una fecha válida.' })
  @Type(() => Date)
  fechaFin?: Date;

  @ApiPropertyOptional({ description: 'ID del estado' })
  @IsOptional()
  @IsNumber({}, { message: 'El estado debe ser un número.' })
  estadoId?: number;
}

export class TurnoDto {
  @ApiProperty()
  id: string;

  @ApiProperty()
  puestoId: number;

  @ApiProperty()
  ticketId: string;

  @ApiProperty()
  fechaInicio: Date;

  @ApiPropertyOptional()
  fechaFin?: Date;

  @ApiProperty()
  estadoId: number;

  @ApiPropertyOptional()
  puestoNavigation?: any;

  @ApiPropertyOptional()
  ticketNavigation?: any;

  @ApiPropertyOptional()
  estadoNavigation?: any;
}
