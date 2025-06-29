# TFG - Plataforma de Gesti�n de Proyectos y Experiencias

Este proyecto es una plataforma web desarrollada en .NET 9 y Blazor WebAssembly para la gesti�n de proyectos colaborativos, usuarios, roles y experiencias educativas (GoRaceExperience). Est� orientada a entornos educativos y de investigaci�n, permitiendo la integraci�n con herramientas externas como GitLab, SonarQube y OpenProject.

## Caracter�sticas principales

- **Gesti�n de Proyectos**: CRUD completo de proyectos, asignaci�n de usuarios, plantillas, m�tricas y KPIs.
- **Gesti�n de Usuarios**: Registro, edici�n, b�squeda y asignaci�n de roles y proyectos.
- **Gesti�n de Roles y Permisos**: Sistema de roles personalizable con permisos granulares.
- **Experiencias GoRace**: CRUD de experiencias educativas, con soporte para experiencias de tipo base, por proyecto o plataforma, usando CQRS y MediatR.
- **Integraciones**: Obtenci�n de m�tricas y datos desde GitLab, SonarQube y OpenProject.
- **Paneles e informes**: Visualizaci�n de KPIs, m�tricas de calidad, cobertura, bugs, code smells, etc.
- **Autenticaci�n JWT**: Seguridad basada en tokens JWT.
- **Arquitectura limpia**: Separaci�n de capas (API, Aplicaci�n, Dominio, Infraestructura, Frontend) y uso de patrones CQRS/MediatR.

## Tecnolog�as utilizadas

- .NET 9, C# 13
- Blazor WebAssembly
- MediatR (CQRS)
- Entity Framework Core
- SQL Server
- MudBlazor (UI)
- GitLab API, SonarQube API, OpenProject API

## Estructura de la soluci�n

- **TFG**: Proyecto principal (API, l�gica de negocio, infraestructura)
- **Front**: Proyecto Blazor WebAssembly (interfaz de usuario)
- **Shared**: DTOs y contratos compartidos
- **TFG.OpenProjectClient / TFG.SonarQubeClient / NGitLab**: Clientes para integraciones externas

## Experiencias GoRace

Las experiencias pueden ser de tres tipos:
- **Base**: Experiencia gen�rica
- **Project**: Asociada a un proyecto concreto
- **Platform**: Asociada a varios proyectos

El CRUD de experiencias se realiza mediante endpoints CQRS y MediatR, con DTOs espec�ficos y filtrado obligatorio por tipo de experiencia.

## Integraciones

- **GitLab**: M�tricas de repositorios y actividad
- **SonarQube**: M�tricas de calidad de c�digo
- **OpenProject**: Gesti�n de tareas y proyectos

## Seguridad

- Autenticaci�n y autorizaci�n basada en JWT
- Permisos y roles personalizables

## C�mo ejecutar

1. Configura la cadena de conexi�n a SQL Server en `appsettings.json`.
2. Ejecuta las migraciones de base de datos.
3. Inicia el backend (`TFG`) y el frontend (`Front`).
4. Accede a la plataforma desde el navegador.

---

Para m�s detalles, consulta la documentaci�n interna de cada proyecto y los comentarios en el c�digo.