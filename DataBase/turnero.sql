-- Tabla: roles
CREATE TABLE roles (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(20) NOT NULL UNIQUE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: usuarios
CREATE TABLE usuarios (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    username VARCHAR(30) NOT NULL UNIQUE,
    rol_id INT UNSIGNED NOT NULL,
    FOREIGN KEY (rol_id) REFERENCES roles(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: clientes
CREATE TABLE clientes (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(10) NOT NULL UNIQUE CHECK (dni REGEXP '^[0-9]+$'),
    titular VARCHAR(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: sectores
CREATE TABLE sectores (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    padre_id INT UNSIGNED DEFAULT NULL,
    letra VARCHAR(3) NULL UNIQUE CHECK (letra REGEXP '^[A-Z]{1,3}$'),
    nombre VARCHAR(50) NULL UNIQUE,
    descripcion VARCHAR(120) NULL,
    FOREIGN KEY (padre_id) REFERENCES sectores(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: mostradores
CREATE TABLE mostradores (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    numero INT UNSIGNED NOT NULL,
    ip VARCHAR(15) NOT NULL UNIQUE,
    tipo VARCHAR(10) NULL,
    sector_id INT UNSIGNED NOT NULL,
    FOREIGN KEY (sector_id) REFERENCES sectores(id),
    UNIQUE (sector_id, numero)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: puestos
CREATE TABLE puestos (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT UNSIGNED NOT NULL,
    mostrador_id INT UNSIGNED NOT NULL,
    login DATETIME,
    logout DATETIME,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (mostrador_id) REFERENCES mostradores(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: estados
CREATE TABLE estados (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    letra VARCHAR(2) NOT NULL UNIQUE CHECK (letra REGEXP '^[A-Z]{1,2}$'),
    descripcion VARCHAR(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: tickets
CREATE TABLE tickets (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    letra VARCHAR(2) NOT NULL CHECK (letra REGEXP '^[A-Z]{1,2}$'),
    numero INT UNSIGNED NOT NULL,
    cliente_id BIGINT UNSIGNED NOT NULL,
    fecha DATETIME NOT NULL,
    sector_id_origen INT UNSIGNED NOT NULL,
    sector_id_actual INT UNSIGNED NOT NULL,
    estado_id INT UNSIGNED NOT NULL,
    actualizado DATETIME NOT NULL,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id),
    FOREIGN KEY (sector_id_origen) REFERENCES sectores(id),
    FOREIGN KEY (sector_id_actual) REFERENCES sectores(id),
    FOREIGN KEY (estado_id) REFERENCES estados(id),
    UNIQUE (letra, numero)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: turnos
CREATE TABLE turnos (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    puesto_id INT UNSIGNED NOT NULL,
    ticket_id BIGINT UNSIGNED NOT NULL,
    fecha_inicio DATETIME NOT NULL,
    fecha_fin DATETIME DEFAULT NULL,
    estado_id INT UNSIGNED NOT NULL,
    FOREIGN KEY (puesto_id) REFERENCES puestos(id),
    FOREIGN KEY (ticket_id) REFERENCES tickets(id),
    FOREIGN KEY (estado_id) REFERENCES estados(id)
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
    FOREIGN KEY (ticket_id) REFERENCES tickets(id),
    FOREIGN KEY (estado_id) REFERENCES estados(id),
    FOREIGN KEY (puesto_id) REFERENCES puestos(id),
    FOREIGN KEY (turno_id) REFERENCES turnos(id),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Índices recomendados para rendimiento
CREATE INDEX idx_puestos_usuario ON puestos(usuario_id);
CREATE INDEX idx_tickets_cliente ON tickets(cliente_id);
CREATE INDEX idx_turnos_ticket ON turnos(ticket_id);
CREATE INDEX idx_historial_ticket ON historiales(ticket_id);
CREATE INDEX idx_ticket_sector_estado ON tickets(sector_id_actual, estado_id);
CREATE INDEX idx_turno_puesto ON turnos(puesto_id);
CREATE INDEX idx_historial_fecha ON historiales(fecha);