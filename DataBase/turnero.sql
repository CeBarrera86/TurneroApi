-- Tabla: roles
CREATE TABLE roles (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(20) NOT NULL UNIQUE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: permisos
CREATE TABLE permisos (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: clientes
CREATE TABLE clientes (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    dni CHAR(10) NOT NULL UNIQUE CHECK (dni REGEXP '^[0-9]+$'),
    titular VARCHAR(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: contenidos
CREATE TABLE contenidos (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    ruta VARCHAR(255) NOT NULL,
    tipo ENUM('imagen', 'video') NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: estados
CREATE TABLE estados (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    letra VARCHAR(2) NOT NULL UNIQUE CHECK (letra REGEXP '^[A-Z]{1,2}$'),
    descripcion VARCHAR(20) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: sectores
CREATE TABLE sectores (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    padre_id INT UNSIGNED DEFAULT NULL,
    letra VARCHAR(3) NULL UNIQUE CHECK (letra REGEXP '^[A-Z]{1,3}$'),
    nombre VARCHAR(50) NULL UNIQUE,
    descripcion VARCHAR(120) NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (padre_id) REFERENCES sectores(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: mostradores
CREATE TABLE mostradores (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    numero INT UNSIGNED NOT NULL,
    ip VARCHAR(15) NOT NULL UNIQUE,
    tipo VARCHAR(10) NULL,
    UNIQUE (numero, ip),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: usuarios
CREATE TABLE usuarios (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    username VARCHAR(30) NOT NULL UNIQUE,
    rol_id INT UNSIGNED NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (rol_id) REFERENCES roles(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: puestos
CREATE TABLE puestos (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT UNSIGNED NOT NULL,
    mostrador_id INT UNSIGNED NOT NULL,
    login DATETIME,
    logout DATETIME,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (mostrador_id) REFERENCES mostradores(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: tickets
CREATE TABLE tickets (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    letra VARCHAR(2) NOT NULL CHECK (letra REGEXP '^[A-Z]{1,2}$'),
    numero INT UNSIGNED NOT NULL,
    cliente_id BIGINT UNSIGNED NOT NULL,
    fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sector_id_origen INT UNSIGNED NOT NULL,
    sector_id_actual INT UNSIGNED NULL,
    estado_id INT UNSIGNED NOT NULL,
    actualizado DATETIME NULL,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id) ON DELETE RESTRICT,
    FOREIGN KEY (sector_id_origen) REFERENCES sectores(id) ON DELETE RESTRICT,
    FOREIGN KEY (sector_id_actual) REFERENCES sectores(id) ON DELETE RESTRICT,
    FOREIGN KEY (estado_id) REFERENCES estados(id) ON DELETE RESTRICT,
    UNIQUE (letra, numero, fecha)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: turnos
CREATE TABLE turnos (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    puesto_id INT UNSIGNED NOT NULL,
    ticket_id BIGINT UNSIGNED NOT NULL,
    fecha_inicio DATETIME NOT NULL,
    fecha_fin DATETIME DEFAULT NULL,
    estado_id INT UNSIGNED NOT NULL,
    FOREIGN KEY (puesto_id) REFERENCES puestos(id) ON DELETE RESTRICT,
    FOREIGN KEY (ticket_id) REFERENCES tickets(id) ON DELETE RESTRICT,
    FOREIGN KEY (estado_id) REFERENCES estados(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: historiales
CREATE TABLE historiales (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    ticket_id BIGINT UNSIGNED NOT NULL,
    estado_id INT UNSIGNED NOT NULL,
    fecha DATETIME NOT NULL,
    puesto_id INT UNSIGNED NULL,
    turno_id BIGINT UNSIGNED NULL,
    usuario_id INT UNSIGNED NULL,
    comentarios VARCHAR(255),
    FOREIGN KEY (ticket_id) REFERENCES tickets(id) ON DELETE RESTRICT,
    FOREIGN KEY (estado_id) REFERENCES estados(id) ON DELETE RESTRICT,
    FOREIGN KEY (puesto_id) REFERENCES puestos(id) ON DELETE RESTRICT,
    FOREIGN KEY (turno_id) REFERENCES turnos(id) ON DELETE RESTRICT,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla intermedia: rol_permiso
CREATE TABLE rol_permiso (
    rol_id INT UNSIGNED NOT NULL,
    permiso_id INT UNSIGNED NOT NULL,
    PRIMARY KEY (rol_id, permiso_id),
    FOREIGN KEY (rol_id) REFERENCES roles(id) ON DELETE RESTRICT,
    FOREIGN KEY (permiso_id) REFERENCES permisos(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla intermedia: mostrador_sector
CREATE TABLE mostrador_sector (
    mostrador_id INT UNSIGNED NOT NULL,
    sector_id INT UNSIGNED NOT NULL,
    PRIMARY KEY (mostrador_id, sector_id),
    FOREIGN KEY (mostrador_id) REFERENCES mostradores(id) ON DELETE RESTRICT,
    FOREIGN KEY (sector_id) REFERENCES sectores(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Índices recomendados
CREATE INDEX idx_puestos_usuario ON puestos(usuario_id);
CREATE INDEX idx_tickets_cliente ON tickets(cliente_id);
CREATE INDEX idx_turnos_ticket ON turnos(ticket_id);
CREATE INDEX idx_historial_ticket ON historiales(ticket_id);
CREATE INDEX idx_ticket_sector_estado ON tickets(sector_id_actual, estado_id);
CREATE INDEX idx_turno_puesto ON turnos(puesto_id);
CREATE INDEX idx_historial_fecha ON historiales(fecha);
CREATE INDEX idx_usuarios_nombre_apellido ON usuarios(nombre, apellido);
CREATE INDEX idx_tickets_fecha ON tickets(fecha);