# NotificationAPI

[![CI/CD](https://github.com/nmino1984/NotificationAPI_VBNet/actions/workflows/ci.yml/badge.svg)](https://github.com/nmino1984/NotificationAPI_VBNet/actions/workflows/ci.yml)
![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?logo=dotnet)
![VB.NET](https://img.shields.io/badge/VB.NET-Domain%20%2B%20Application-0078d7)
![C#](https://img.shields.io/badge/C%23-Infrastructure%20%2B%20API-239120?logo=csharp)
![Tests](https://img.shields.io/badge/tests-14%20passing-brightgreen)
![Docker](https://img.shields.io/badge/Docker-Alpine-2496ED?logo=docker)

> 🇺🇸 [English](#-english) · 🇪🇸 [Español](#-español)

---

## 🇺🇸 English

### Overview

REST API for user and notification management, built with **.NET 10** following **Clean Architecture** principles. The project deliberately combines two .NET languages — **VB.NET** in the inner layers (Domain and Application) and **C#** in the outer layers (Infrastructure and API) — to demonstrate cross-language interoperability in the same solution.

---

### Architecture

```
┌──────────────────────────────────────────────────────────────┐
│                      HTTP Request                            │
└─────────────────────────┬────────────────────────────────────┘
                          │
        ┌─────────────────▼──────────────────┐
        │          API Layer  (C#)           │
        │  Controllers · Swagger · Validation│
        └─────────────────┬──────────────────┘
                          │
        ┌─────────────────▼──────────────────┐
        │      Application Layer (VB.NET)    │
        │   Use Cases · AutoMapper · DTOs    │
        └──────────┬──────────────┬──────────┘
                   │              ��
   ┌───────────────▼──┐   ┌───────▼──────────────────┐
   │  Domain (VB.NET) │   │  Infrastructure  (C#)    │
   │ Entities · IRepos│   │ EF Core · SQLite · Repos │
   └──────────────────┘   └──────────────────────────┘
```

**Dependency rule:** each layer only knows about the layer directly below it. Domain has zero external dependencies.

---

### Tech Stack

| Layer | Language | Key Libraries |
|-------|----------|---------------|
| Domain | VB.NET | — |
| Application | VB.NET | AutoMapper 16.1.1, FluentValidation 12.1.1 |
| Infrastructure | C# | EF Core 10.0.7, SQLite |
| API | C# | ASP.NET Core 10, Swashbuckle 10.1.7 |
| Tests | C# | xUnit 2.9, Moq 4.20, EF Core InMemory |
| CI/CD | — | GitHub Actions, Docker (Alpine), GHCR |

---

### Folder Structure

```
NotificationAPI/
├── NotificationAPI.Domain/           # VB.NET — zero dependencies
│   ├── Entities/
│   │   ├── User.vb
│   │   └── Notification.vb
│   └── Repositories/
│       └── IRepository.vb            # IUserRepository, INotificationRepository, IUnitOfWork
│
├── NotificationAPI.Application/      # VB.NET — depends on Domain only
│   ├── DTOs/
│   │   ├── Requests/                 # CreateUserRequest, SendNotificationRequest
│   │   └── Responses/                # UserResponse, NotificationResponse
│   ├── Mappings/
│   │   └── MappingProfile.vb
│   ├── UseCases/
│   │   ├── Users/GetUserByIdUseCase.vb
│   │   └── Notifications/SendNotificationUseCase.vb
│   └── Validators/
│       ├── CreateUserValidator.vb
│       └── SendNotificationValidator.vb
│
├── NotificationAPI.Infrastructure/   # C# — implements Domain interfaces
│   └── Data/
│       ├── ApplicationDbContext.cs
│       ├── Migrations/
│       └── Repositories/
│           ├── UserRepository.cs
│           ├── NotificationRepository.cs
│           └── UnitOfWork.cs
│
├── NotificationAPI.API/              # C# — entry point
│   ├── Controllers/
│   │   ├── UsersController.cs
│   │   └── NotificationsController.cs
│   └── Program.cs
│
├── NotificationAPI.Tests/            # C# — xUnit (14 tests)
│   ├── Validators/
│   ├── UseCases/
│   ├── Repositories/
│   └── UnitOfWork/
│
├── Dockerfile
├── .dockerignore
└── .github/
    └── workflows/
        └── ci.yml
```

---

### Getting Started

#### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- No database setup required — SQLite file is created automatically on first run

#### Run locally

```bash
git clone https://github.com/nmino1984/NotificationAPI_VBNet.git
cd NotificationAPI_VBNet

dotnet restore NotificationAPI.slnx
dotnet run --project NotificationAPI.API/NotificationAPI.API.csproj
```

API available at `http://localhost:5000`  
Swagger UI at `http://localhost:5000/swagger`

#### Run tests

```bash
dotnet test NotificationAPI.Tests/NotificationAPI.Tests.csproj --verbosity normal
```

Expected output: **14 tests passed**

#### Run with Docker

```bash
docker build -t notificationapi:latest .
docker run -p 5000:5000 notificationapi:latest
```

Or pull the published image from GHCR:

```bash
docker pull ghcr.io/nmino1984/notificationapi_vbnet:latest
docker run -p 5000:5000 ghcr.io/nmino1984/notificationapi_vbnet:latest
```

---

### API Endpoints

#### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/users` | Create a new user |
| `GET` | `/api/users` | Get all users |
| `GET` | `/api/users/{id}` | Get user by ID |

#### Notifications

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/notifications/send` | Send a notification |
| `GET` | `/api/notifications/{id}` | Get notification by ID |
| `GET` | `/api/notifications/user/{userId}` | Get all notifications for a user |

#### Examples

```bash
# Create a user
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "email": "john@example.com"}'

# Response 201
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "John Doe",
  "email": "john@example.com",
  "createdAt": "2026-04-29T12:00:00Z"
}

# Send a notification
curl -X POST http://localhost:5000/api/notifications/send \
  -H "Content-Type: application/json" \
  -d '{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "Welcome", "message": "Thanks for joining!"}'

# Get user's notifications
curl http://localhost:5000/api/notifications/user/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

---

### Tests — 14 Passing

| Category | File | Tests |
|----------|------|-------|
| Validators | `CreateUserValidatorTests.cs` | Valid request passes · Invalid request fails |
| Validators | `SendNotificationValidatorTests.cs` | Valid request passes · Invalid request fails |
| Use Cases | `GetUserByIdUseCaseTests.cs` | User found returns response · User not found throws exception |
| Use Cases | `SendNotificationUseCaseTests.cs` | Valid request returns response · User not found throws exception |
| Repositories | `UserRepositoryTests.cs` | Soft delete works · GetAll filters deleted |
| Repositories | `NotificationRepositoryTests.cs` | Orders by date descending · Filters deleted |
| Unit of Work | `UnitOfWorkTests.cs` | SaveChangesAsync persists · Users lazy init returns same instance |

---

### CI/CD Pipeline

Every push to `main` triggers two jobs:

```
CI/CD
├── Build & Test    → dotnet restore → dotnet build → 14 xUnit tests
└── Docker Build    → (runs only if tests pass) → builds image → pushes to GHCR
```

The Docker image is published automatically to:  
`ghcr.io/nmino1984/notificationapi_vbnet:latest`

---

### Technical Decisions

**Why VB.NET for Domain and Application?**

The project was originally designed entirely in VB.NET as a showcase of enterprise-grade VB.NET development on modern .NET. EF Core's migration tooling (`IMigrationsCodeGenerator`) does not support VB.NET — it generates C# files unconditionally. Keeping the Infrastructure and API layers in C# is the pragmatic solution used in real-world .NET shops that maintain VB.NET codebases, and it demonstrates cross-language interoperability rather than hiding it.

**Why Clean Architecture?**

The dependency rule ensures the Domain and Application layers (business logic) are completely isolated from infrastructure concerns. This makes them trivially testable with mocks — no database, no HTTP, no framework.

**Why SQLite?**

Zero configuration for development and testing. The production path is a one-line change in `Program.cs` and `appsettings.json` to switch to SQL Server or PostgreSQL.

---

### Author

**Noel Miño**  
📧 noelminoherrera@gmail.com  
🐙 [github.com/nmino1984](https://github.com/nmino1984)

---
---

## 🇪🇸 Español

### Descripción general

API REST para gestión de usuarios y notificaciones, construida con **.NET 10** siguiendo los principios de **Clean Architecture**. El proyecto combina deliberadamente dos lenguajes .NET — **VB.NET** en las capas internas (Domain y Application) y **C#** en las capas externas (Infrastructure y API) — para demostrar la interoperabilidad entre lenguajes en una misma solución.

---

### Arquitectura

```
┌──────────────────────────────────────────────────────────────┐
│                     Petición HTTP                            │
└─────────────────────────┬────────────────────────────────────┘
                          │
        ┌─────────────────▼──────────────────┐
        │          Capa API  (C#)            │
        │  Controllers · Swagger · Validación│
        └─────────────────┬──────────────────┘
                          │
        ┌─────────────────▼──────────────────┐
        │    Capa Application (VB.NET)       │
        │   Casos de Uso · AutoMapper · DTOs │
        └──────────┬──────────��───┬──────────┘
                   │              │
   ┌───────────────▼──┐   ┌───────▼──────────────────┐
   │ Domain (VB.NET)  │   │  Infrastructure  (C#)    │
   │ Entidades · IRepo│   │ EF Core · SQLite · Repos │
   └──────────────────┘   └──────────────────────────┘
```

**Regla de dependencia:** cada capa solo conoce a la capa inmediatamente inferior. Domain no tiene ninguna dependencia externa.

---

### Stack Tecnológico

| Capa | Lenguaje | Librerías principales |
|------|----------|-----------------------|
| Domain | VB.NET | — |
| Application | VB.NET | AutoMapper 16.1.1, FluentValidation 12.1.1 |
| Infrastructure | C# | EF Core 10.0.7, SQLite |
| API | C# | ASP.NET Core 10, Swashbuckle 10.1.7 |
| Tests | C# | xUnit 2.9, Moq 4.20, EF Core InMemory |
| CI/CD | — | GitHub Actions, Docker (Alpine), GHCR |

---

### Cómo ejecutar

#### Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- No se requiere configuración de base de datos — el archivo SQLite se crea automáticamente al arrancar

#### En local

```bash
git clone https://github.com/nmino1984/NotificationAPI_VBNet.git
cd NotificationAPI_VBNet

dotnet restore NotificationAPI.slnx
dotnet run --project NotificationAPI.API/NotificationAPI.API.csproj
```

API disponible en `http://localhost:5000`  
Swagger UI en `http://localhost:5000/swagger`

#### Tests

```bash
dotnet test NotificationAPI.Tests/NotificationAPI.Tests.csproj --verbosity normal
```

Resultado esperado: **14 tests pasados**

#### Con Docker

```bash
docker build -t notificationapi:latest .
docker run -p 5000:5000 notificationapi:latest
```

---

### Endpoints

#### Usuarios

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/users` | Crear nuevo usuario |
| `GET` | `/api/users` | Obtener todos los usuarios |
| `GET` | `/api/users/{id}` | Obtener usuario por ID |

#### Notificaciones

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/notifications/send` | Enviar una notificación |
| `GET` | `/api/notifications/{id}` | Obtener notificación por ID |
| `GET` | `/api/notifications/user/{userId}` | Obtener todas las notificaciones de un usuario |

---

### Tests — 14 Pasando

| Categoría | Archivo | Tests |
|-----------|---------|-------|
| Validadores | `CreateUserValidatorTests.cs` | Request válido pasa · Request inválido falla |
| Validadores | `SendNotificationValidatorTests.cs` | Request válido pasa · Request inválido falla |
| Casos de Uso | `GetUserByIdUseCaseTests.cs` | Usuario encontrado devuelve respuesta · No encontrado lanza excepción |
| Casos de Uso | `SendNotificationUseCaseTests.cs` | Request válido devuelve respuesta · Usuario no encontrado lanza excepción |
| Repositorios | `UserRepositoryTests.cs` | Soft delete funciona · GetAll filtra eliminados |
| Repositorios | `NotificationRepositoryTests.cs` | Ordena por fecha descendente · Filtra eliminados |
| Unit of Work | `UnitOfWorkTests.cs` | SaveChangesAsync persiste · Lazy init devuelve misma instancia |

---

### Decisiones Técnicas

#### ¿Por qué VB.NET en Domain y Application?

El proyecto fue diseñado originalmente en VB.NET al completo como demostración de desarrollo empresarial con VB.NET moderno. El tooling de migraciones de EF Core (`IMigrationsCodeGenerator`) no soporta VB.NET — genera los archivos de migración exclusivamente en C#. Mantener Infrastructure y API en C# es la solución pragmática que se usa en empresas reales que mantienen bases de código VB.NET, y demuestra interoperabilidad entre lenguajes en lugar de ocultarla.

Esta decisión también refleja una situación real muy común en la industria: sistemas legados en VB.NET que se integran con capas nuevas en C# sin necesidad de reescribir la lógica de negocio existente.

#### ¿Por qué Clean Architecture?

La regla de dependencia garantiza que las capas Domain y Application (lógica de negocio) estén completamente aisladas de las preocupaciones de infraestructura. Esto las hace trivialmente testeables con mocks — sin base de datos, sin HTTP, sin framework.

#### ¿Por qué SQLite?

Cero configuración para desarrollo y testing. El camino a producción es un cambio de una línea en `Program.cs` y `appsettings.json` para cambiar a SQL Server o PostgreSQL.

---

### Pipeline CI/CD

Cada push a `main` dispara dos jobs:

```
CI/CD
├── Build & Test    → dotnet restore → dotnet build → 14 tests xUnit
└── Docker Build    → (solo si los tests pasan) → construye imagen → publica en GHCR
```

La imagen Docker se publica automáticamente en:  
`ghcr.io/nmino1984/notificationapi_vbnet:latest`

---

### Autor

**Noel Miño**  
📧 noelminoherrera@gmail.com  
🐙 [github.com/nmino1984](https://github.com/nmino1984)
