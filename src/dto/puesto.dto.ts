import { IsNotEmpty, IsString, IsNumber, IsBoolean, IsOptional, IsDate } from 'class-validator';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { Type } from 'class-transformer';

export class PuestoCrearDto {
  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El usuario es obligatorio.' })
  @IsNumber({}, { message: 'El usuario debe ser un número.' })
  usuarioId: number;

  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El mostrador es obligatorio.' })
  @IsNumber({}, { message: 'El mostrador debe ser un número.' })
  mostradorId: number;
}

export class PuestoActualizarDto {
  @ApiPropertyOptional()
  @IsOptional()
  @IsDate({ message: 'El login debe ser una fecha válida.' })
  @Type(() => Date)
  login?: Date;

  @ApiPropertyOptional()
  @IsOptional()
  @IsDate({ message: 'El logout debe ser una fecha válida.' })
  @Type(() => Date)
  logout?: Date;

  @ApiPropertyOptional()
  @IsOptional()
  @IsBoolean({ message: 'Activo debe ser un booleano.' })
  activo?: boolean;
}

export class PuestoDto {
  @ApiProperty()
  id: number;

  @ApiProperty()
  usuarioId: number;

  @ApiProperty()
  mostradorId: number;

  @ApiPropertyOptional()
  login?: Date;

  @ApiPropertyOptional()
  logout?: Date;

  @ApiProperty()
  activo: boolean;
}

export class PuestoLoginDto {
  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El usuario es obligatorio.' })
  @IsNumber({}, { message: 'El usuario debe ser un número.' })
  usuarioId: number;

  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El mostrador es obligatorio.' })
  @IsNumber({}, { message: 'El mostrador debe ser un número.' })
  mostradorId: number;
}
