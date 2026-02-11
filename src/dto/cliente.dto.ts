import { IsNotEmpty, IsString, IsOptional } from 'class-validator';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';

export class ClienteCrearDto {
  @ApiProperty({ example: '12345678' })
  @IsNotEmpty({ message: 'El DNI es obligatorio.' })
  @IsString({ message: 'El DNI debe ser un string.' })
  dni: string;

  @ApiProperty({ example: 'Juan Pérez' })
  @IsNotEmpty({ message: 'El titular es obligatorio.' })
  @IsString({ message: 'El titular debe ser un string.' })
  titular: string;
}

export class ClienteActualizarDto {
  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'El DNI debe ser un string.' })
  dni?: string;

  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'El titular debe ser un string.' })
  titular?: string;
}

export class ClienteDto {
  @ApiProperty()
  id: string;

  @ApiProperty()
  dni: string;

  @ApiProperty()
  titular: string;
}
