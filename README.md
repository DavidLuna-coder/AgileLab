# TFG - Plataforma de Gestión de Proyectos y Experiencias

Este proyecto es una plataforma web desarrollada en .NET 9 y Blazor WebAssembly para la gestión de proyectos colaborativos, usuarios, roles y experiencias educativas (GoRaceExperience). Está orientada a entornos educativos y de investigación, permitiendo la integración con herramientas externas como GitLab, SonarQube y OpenProject.

## Características principales

- **Gestión de Proyectos**: CRUD completo de proyectos, asignación de usuarios, plantillas, métricas y KPIs.
- **Gestión de Usuarios**: Registro, edición, búsqueda y asignación de roles y proyectos.
- **Gestión de Roles y Permisos**: Sistema de roles personalizable con permisos granulares.
- **Experiencias GoRace**: CRUD de experiencias educativas, con soporte para experiencias de tipo base, por proyecto o plataforma, usando CQRS y MediatR.
- **Integraciones**: Obtención de métricas y datos desde GitLab, SonarQube y OpenProject.
- **Paneles e informes**: Visualización de KPIs, métricas de calidad, cobertura, bugs, code smells, etc.
- **Autenticación JWT**: Seguridad basada en tokens JWT.
- **Arquitectura limpia**: Separación de capas (API, Aplicación, Dominio, Infraestructura, Frontend) y uso de patrones CQRS/MediatR.

## Tecnologías utilizadas

- .NET 9, C# 13
- Blazor WebAssembly
- MediatR (CQRS)
- Entity Framework Core
- SQL Server
- MudBlazor (UI)
- GitLab API, SonarQube API, OpenProject API

## Estructura de la solución

- **TFG**: Proyecto principal (API, lógica de negocio, infraestructura)
- **Front**: Proyecto Blazor WebAssembly (interfaz de usuario)
- **Shared**: DTOs y contratos compartidos
- **TFG.OpenProjectClient / TFG.SonarQubeClient / NGitLab**: Clientes para integraciones externas

## Experiencias GoRace

Las experiencias pueden ser de tres tipos:
- **Base**: Experiencia genérica
- **Project**: Asociada a un proyecto concreto
- **Platform**: Asociada a varios proyectos

El CRUD de experiencias se realiza mediante endpoints CQRS y MediatR, con DTOs específicos y filtrado obligatorio por tipo de experiencia.

## Integraciones

- **GitLab**: Métricas de repositorios y actividad
- **SonarQube**: Métricas de calidad de código
- **OpenProject**: Gestión de tareas y proyectos

## Seguridad

- Autenticación y autorización basada en JWT
- Permisos y roles personalizables

## Cómo ejecutar

1. Configura la cadena de conexión a SQL Server en `appsettings.json`.
2. Ejecuta las migraciones de base de datos.
3. Inicia el backend (`TFG`) y el frontend (`Front`).
4. Accede a la plataforma desde el navegador.

---

![image](https://github.com/user-attachments/assets/5a7f17ad-6040-4ee7-a694-f79b961ea704)
![image](https://github.com/user-attachments/assets/110ee142-b7a0-4568-82d2-7298b80e9baf)

