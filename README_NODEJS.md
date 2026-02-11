# Turnero API - Node.js con TypeScript

Sistema de gestión de turnos migrado de ASP.NET Core a Node.js con NestJS y TypeScript.

## 🚀 Tecnologías Utilizadas

- **Framework**: NestJS 10.x
- **Lenguaje**: TypeScript 5.x
- **Base de Datos**: MySQL 8.x con TypeORM
- **Autenticación**: JWT (JSON Web Tokens)
- **Validación**: class-validator y class-transformer
- **Documentación**: Swagger/OpenAPI
- **Logging**: Winston
- **Testing**: Jest

## 📋 Requisitos Previos

- Node.js >= 18.x
- npm >= 9.x o yarn >= 1.22.x
- MySQL >= 8.0
- Git

## 🔧 Instalación

### 1. Clonar el repositorio

```bash
git clone <repository-url>
cd TurneroApi
```

### 2. Instalar dependencias

```bash
npm install
```

### 3. Configurar variables de entorno

Copiar el archivo `.env.example` a `.env` y configurar las variables:

```bash
cp .env.example .env
```

Editar el archivo `.env` con tus configuraciones:

```env
# Application
NODE_ENV=development
PORT=3000

# Database - Turnero
DB_HOST=127.0.0.1
DB_PORT=3306
DB_USERNAME=root
DB_PASSWORD=tu_password
DB_DATABASE=turnero_db

# JWT
JWT_SECRET=tu_clave_secreta_muy_segura_aqui
JWT_ISSUER=Api
JWT_AUDIENCE=Api
JWT_EXPIRATION=60m

# CORS
CORS_ORIGINS=http://localhost:5173,http://localhost:5174
```

### 4. Crear la base de datos

```bash
mysql -u root -p
CREATE DATABASE turnero_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 5. Ejecutar migraciones

```bash
npm run migration:run
```

## 🏃 Ejecución

### Modo Desarrollo

```bash
npm run start:dev
```

La aplicación estará disponible en: `http://localhost:3000/api`

Documentación Swagger: `http://localhost:3000/api/docs`

### Modo Producción

```bash
# Compilar
npm run build

# Ejecutar
npm run start:prod
```

## 📁 Estructura del Proyecto

```
src/
├── auth/                   # Autenticación JWT
│   ├── jwt.strategy.ts
│   └── ...
├── config/                 # Configuraciones
│   └── typeorm.config.ts
├── decorators/             # Decoradores personalizados
│   ├── current-user.decorator.ts
│   ├── permissions.decorator.ts
│   └── api-pagination.decorator.ts
├── dto/                    # Data Transfer Objects
│   ├── turno.dto.ts
│   ├── ticket.dto.ts
│   └── ...
├── entities/               # Entidades TypeORM
│   ├── turno.entity.ts
│   ├── ticket.entity.ts
│   └── ...
├── enums/                  # Enumeraciones
│   ├── estado-ticket.enum.ts
│   └── sector.enum.ts
├── filters/                # Exception Filters
│   └── http-exception.filter.ts
├── guards/                 # Guards de autorización
│   ├── jwt-auth.guard.ts
│   └── permissions.guard.ts
├── modules/                # Módulos de la aplicación
│   ├── turno/
│   │   ├── turno.controller.ts
│   │   ├── turno.service.ts
│   │   └── turno.module.ts
│   ├── cliente/
│   ├── ticket/
│   └── ...
├── utils/                  # Utilidades
│   └── pagination.helper.ts
├── app.module.ts           # Módulo principal
└── main.ts                 # Punto de entrada
```

## 🔑 Autenticación y Autorización

### Obtener Token JWT

```bash
POST /api/auth/login
Content-Type: application/json

{
  "username": "usuario",
  "password": "password"
}
```

### Usar Token en Requests

```bash
GET /api/turno
Authorization: Bearer <tu_token_jwt>
```

### Sistema de Permisos

El sistema utiliza permisos basados en roles. Los permisos se definen con el decorador `@RequirePermissions()`:

```typescript
@Get()
@RequirePermissions('ver_turno')
async findAll() { ... }
```

## 📝 API Endpoints Principales

### Turnos

- `GET /api/turno` - Listar turnos (paginado)
- `GET /api/turno/:id` - Obtener turno por ID
- `GET /api/turno/puesto/:puestoId/activo` - Obtener turno activo por puesto
- `POST /api/turno` - Crear turno
- `PUT /api/turno/:id` - Actualizar turno
- `DELETE /api/turno/:id` - Eliminar turno

### Clientes

- `GET /api/cliente` - Listar clientes
- `GET /api/cliente/:id` - Obtener cliente
- `POST /api/cliente` - Crear cliente
- `PUT /api/cliente/:id` - Actualizar cliente
- `DELETE /api/cliente/:id` - Eliminar cliente

### Estados

- `GET /api/estado` - Listar estados
- `GET /api/estado/:id` - Obtener estado
- `POST /api/estado` - Crear estado
- `PUT /api/estado/:id` - Actualizar estado
- `DELETE /api/estado/:id` - Eliminar estado

## 🧪 Testing

```bash
# Tests unitarios
npm run test

# Tests e2e
npm run test:e2e

# Coverage
npm run test:cov
```

## 📊 Migraciones desde ASP.NET Core

### Equivalencias de Tecnologías

| ASP.NET Core | Node.js/NestJS |
|--------------|----------------|
| Entity Framework Core | TypeORM |
| AutoMapper | class-transformer |
| FluentValidation | class-validator |
| Serilog | Winston |
| Swagger | @nestjs/swagger |
| JWT Bearer Auth | @nestjs/jwt + passport-jwt |
| SignalR | Socket.io (por implementar) |

### Cambios Principales

1. **IDs de tipo ulong/bigint**: En MySQL se manejan como strings en TypeScript
2. **DateTime**: Se usan objetos `Date` de JavaScript
3. **Nullable types**: Se usan tipos opcionales con `?` en TypeScript
4. **Decoradores**: NestJS usa decoradores similares a ASP.NET Core
5. **Inyección de dependencias**: NestJS usa el mismo patrón que ASP.NET Core

## 🔨 Tareas Pendientes

Los siguientes módulos tienen estructura básica pero requieren implementación completa:

- [ ] Módulo Ticket (service + controller)
- [ ] Módulo Puesto (service + controller)
- [ ] Módulo Usuario (service + controller)
- [ ] Módulo Historial (service + controller)
- [ ] Módulo Mostrador (service + controller)
- [ ] Módulo Sector (service + controller)
- [ ] Módulo Permiso (service + controller)
- [ ] Módulo Rol (service + controller)
- [ ] Módulo Contenido (service + controller)
- [ ] WebSocket/Socket.io para reemplazar SignalR
- [ ] Servicios externos (GeaPico, Totem)
- [ ] Manejo de archivos e imágenes
- [ ] Tests unitarios y e2e

## 📚 Recursos

- [Documentación de NestJS](https://docs.nestjs.com/)
- [TypeORM Documentation](https://typeorm.io/)
- [Class Validator](https://github.com/typestack/class-validator)
- [Passport JWT](http://www.passportjs.org/packages/passport-jwt/)

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama de feature (`git checkout -b feature/AmazingFeature`)
3. Commit los cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto es privado y confidencial.

## 👥 Equipo

- Desarrollo original (ASP.NET Core): [Team]
- Migración a Node.js/TypeScript: [Team]

---

Para más información o soporte, contactar al equipo de desarrollo.
