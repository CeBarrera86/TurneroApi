import { IsNotEmpty, IsString, IsNumber, IsBoolean, IsOptional } from 'class-validator';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';

export class UsuarioCrearDto {
  @ApiProperty({ example: 'Juan' })
  @IsNotEmpty({ message: 'El nombre es obligatorio.' })
  @IsString({ message: 'El nombre debe ser un string.' })
  nombre: string;

  @ApiProperty({ example: 'Pérez' })
  @IsNotEmpty({ message: 'El apellido es obligatorio.' })
  @IsString({ message: 'El apellido debe ser un string.' })
  apellido: string;

  @ApiProperty({ example: 'jperez' })
  @IsNotEmpty({ message: 'El username es obligatorio.' })
  @IsString({ message: 'El username debe ser un string.' })
  username: string;

  @ApiProperty({ example: 1 })
  @IsNotEmpty({ message: 'El rol es obligatorio.' })
  @IsNumber({}, { message: 'El rol debe ser un número.' })
  rolId: number;

  @ApiPropertyOptional({ example: true })
  @IsOptional()
  @IsBoolean({ message: 'Activo debe ser un booleano.' })
  activo?: boolean;
}

export class UsuarioActualizarDto {
  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'El nombre debe ser un string.' })
  nombre?: string;

  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'El apellido debe ser un string.' })
  apellido?: string;

  @ApiPropertyOptional()
  @IsOptional()
  @IsNumber({}, { message: 'El rol debe ser un número.' })
  rolId?: number;

  @ApiPropertyOptional()
  @IsOptional()
  @IsBoolean({ message: 'Activo debe ser un booleano.' })
  activo?: boolean;
}

export class UsuarioDto {
  @ApiProperty()
  id: number;

  @ApiProperty()
  nombre: string;

  @ApiProperty()
  apellido: string;

  @ApiProperty()
  username: string;

  @ApiProperty()
  rolId: number;

  @ApiProperty()
  activo: boolean;

  @ApiProperty()
  createdAt: Date;

  @ApiProperty()
  updatedAt: Date;
}
