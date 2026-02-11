import { IsNotEmpty, IsString, IsBoolean, IsOptional } from 'class-validator';
import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';

export class EstadoCrearDto {
  @ApiProperty({ example: 'A' })
  @IsNotEmpty({ message: 'La letra es obligatoria.' })
  @IsString({ message: 'La letra debe ser un string.' })
  letra: string;

  @ApiProperty({ example: 'ATENDIDO' })
  @IsNotEmpty({ message: 'La descripción es obligatoria.' })
  @IsString({ message: 'La descripción debe ser un string.' })
  descripcion: string;
}

export class EstadoActualizarDto {
  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'La letra debe ser un string.' })
  letra?: string;

  @ApiPropertyOptional()
  @IsOptional()
  @IsString({ message: 'La descripción debe ser un string.' })
  descripcion?: string;
}

export class EstadoDto {
  @ApiProperty()
  id: number;

  @ApiProperty()
  letra: string;

  @ApiProperty()
  descripcion: string;

  @ApiProperty()
  createdAt: Date;

  @ApiProperty()
  updatedAt: Date;
}
