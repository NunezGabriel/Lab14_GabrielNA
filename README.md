# Lab 14 – Despliegue de una API .NET (GabrielNA)

API REST de reportes en Excel (ClosedXML) sobre la base de datos `linqexample`, con arquitectura hexagonal, patrón repositorio y CQRS (MediatR). Es la aplicación del Lab 13, preparada para desplegarse.

## Despliegue

| Etapa | Cómo |
|---|---|
| Publicación manual | `dotnet publish Lab13_GabrielNA/Lab13_GabrielNA.csproj -c Release -o ./published` |
| Integración continua | GitHub Actions (`.github/workflows/dotnet.yml`): restore, build, publish, imagen Docker y prueba del contenedor contra PostgreSQL |
| Servidor | Render (`render.yaml`): web service Docker + PostgreSQL. Se despliega solo cuando los checks de GitHub Actions pasan |

## Endpoints

- `GET /api/Clientes`
- `GET /api/Reportes/ventas` y `GET /api/Reportes/ventas/excel` (filtros `clienteId`, `desde`, `hasta`)
- `GET /api/Reportes/productos` y `GET /api/Reportes/productos/excel` (filtro `soloVendidos`)
- `GET /health`

Swagger se abre en la raíz del sitio.

## Ejecutar en local

```bash
dotnet run --project Lab13_GabrielNA --launch-profile http
```

Si la base de datos está vacía, la API crea las tablas y carga los datos de ejemplo (`Lab13_GabrielNA.Infrastructure/Scripts/linqexample.sql`).
