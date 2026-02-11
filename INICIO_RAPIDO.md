# Inicio Rápido - TurneroApi Node.js/TypeScript

## 1. Instalar Dependencias

```bash
npm install
```

## 2. Configurar Base de Datos

Asegúrate de tener MySQL instalado y crea la base de datos:

```sql
CREATE DATABASE db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

## 3. Configurar Variables de Entorno

El archivo `.env` ya está creado con valores por defecto. Edítalo según tu ambiente:

```bash
# Editar configuración de base de datos
DB_HOST=127.0.0.1
DB_PORT=3306
DB_USERNAME=root
DB_PASSWORD=tu_password
DB_DATABASE=db

# Cambiar JWT_SECRET para producción
JWT_SECRET=tu_clave_super_secreta
```

## 4. Ejecutar Migraciones (Opcional)

Si hay una base de datos existente del proyecto .NET, puedes usarla directamente.
Si necesitas crear la estructura desde cero:

```bash
npm run migration:run
```

O usa el script SQL en `DataBase/turnero.sql` del proyecto original.

## 5. Iniciar la Aplicación

### Modo Desarrollo (con hot-reload)

```bash
npm run start:dev
```

### Modo Producción

```bash
npm run build
npm run start:prod
```

## 6. Verificar

Una vez iniciado, verás en consola:

```
🚀 Application is running on: http://localhost:3000/api
📚 Swagger documentation: http://localhost:3000/api/docs
```

Abre tu navegador en `http://localhost:3000/api/docs` para ver la documentación Swagger.

## 7. Probar un Endpoint

Con la API corriendo, prueba:

```bash
# Listar estados (requiere autenticación)
curl -X GET http://localhost:3000/api/estado \
  -H "Authorization: Bearer TU_TOKEN_JWT"
```

## Problemas Comunes

### Error de conexión a MySQL

```
Error: connect ECONNREFUSED 127.0.0.1:3306
```

**Solución**: Verifica que MySQL esté corriendo y las credenciales en `.env` sean correctas.

### Error de módulos no encontrados

```
Error: Cannot find module 'nest-winston'
```

**Solución**: Ejecuta `npm install` nuevamente.

### Puertos en uso

```
Error: listen EADDRINUSE: address already in use :::3000
```

**Solución**: Cambia el puerto en `.env` o detén el proceso que está usando el puerto 3000.

## Siguiente Paso: Desarrollo

Los módulos base están creados. Para continuar el desarrollo:

1. Ver [README_NODEJS.md](README_NODEJS.md) para documentación completa
2. Ver [MIGRACION_RESUMEN.md](MIGRACION_RESUMEN.md) para estado de migración
3. Implementar servicios y controladores pendientes siguiendo el patrón de `TurnoModule`

## Estructura de Ejemplo - Implementar Nuevo Módulo

Para implementar el módulo de Tickets completamente:

1. Ir a `src/modules/ticket/`
2. Crear `ticket.service.ts` (ver `turno.service.ts` como ejemplo)
3. Crear `ticket.controller.ts` (ver `turno.controller.ts` como ejemplo)
4. Actualizar `ticket.module.ts` para incluir providers y controllers

¡Listo para desarrollar! 🚀
