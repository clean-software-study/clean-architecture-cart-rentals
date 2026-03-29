> **Educational proyect:**  The architectural complexity (DDD, Clean Architecture, domain events, etc.) — in a real system of this size it would be over-engineering.

---

# Clean Architecture + DDD — Vehicle Rental System

## Clean Architecture

Clean Architecture organizes code in **concentric layers** where dependencies always point inward — outer layers know about inner layers, never the reverse. The goal is to keep business logic completely independent of frameworks, databases, and delivery mechanisms.

```
* dependencies flow inward only
API / Presentation   ← HTTP, gRPC, WebSockets
Application          ← Use Cases, CQRS, Event Handlers
Infrastructure       ← DB, External APIs, Messaging
Domain               ← Business rules (zero dependencies)
```

## Domain-Driven Design (DDD)

is a design approach focused on modeling the **core business domain** through a shared language between developers and domain experts. Key building blocks:

| Concept | Description |
|---|---|
| **Ubiquitous Language** | A common vocabulary used in both code and conversations with domain experts |
| **Bounded Context** | An explicit boundary within which a single domain model is valid |
| **Aggregate** | A cluster of objects treated as a single unit with one **Aggregate Root** controlling all access |
| **Entity** | An object with a unique identity that persists over time (e.g., `Rental`, `User`) |
| **Value Object** | An immutable object defined by its attributes, not identity (e.g., `Money`, `DateRange`) |
| **Domain Event** | A record of something meaningful that happened in the domain (e.g., `RentalReservedDomainEvent`) |
| **Domain Service** | Stateless logic that doesn't naturally belong to a single entity (e.g., `PriceService`) |
| **Repository** | An abstraction for persisting and retrieving aggregates — interface in Domain, impl in Infrastructure |
| **Factory** | Encapsulates complex aggregate creation logic |

## Layer Responsibilities

### Domain
The innermost layer. Contains **only** business rules — no framework dependencies whatsoever.
- Entities, Value Objects, Aggregates & Aggregate Roots
- Domain Events and their interfaces
- Domain Services
- Repository interfaces (`IUserRepository`, `IVehicleRepository`)
- `IUnitOfWork` interface

### Application
Orchestrates use cases. Depends on Domain; knows nothing about infrastructure or HTTP.
- **Commands & Queries** (CQRS pattern via MediatR)
- **Command/Query Handlers**
- **Domain Event Handlers**
- Application-level DTOs and mapping
- Validation (FluentValidation)
- `IUnitOfWork` usage (not implementation)

### Infrastructure
Implements the technical concerns defined by inner layers.
- EF Core `DbContext` and migrations
- Repository implementations
- External HTTP clients (payment gateway, notification services)
- Message broker integration (RabbitMQ, Kafka)
- Caching (Redis, in-memory)
- Authentication providers (identity, JWT, OAuth)
- File storage (S3, blob storage)

### API / Presentation
The entry point. Deals only with HTTP concerns.
- Controllers or Minimal API endpoints
- Authentication & Authorization middleware
- Request/response serialization
- Global exception handling middleware
- OpenAPI / Swagger setup
- Rate limiting, CORS, health checks

### Optional Layers (larger systems)
| Layer | Purpose |
|---|---|
| **Contracts** | Shared request/response types for API consumers or inter-service communication |
| **Shared Kernel** | Types shared across multiple Bounded Contexts (e.g., `Currency`, base domain events) |
| **Workers / Jobs** | Background processing, scheduled tasks (Hangfire, Quartz.NET) |
| **Tests** | Unit (Domain + Application), Integration (Infrastructure + API), Architecture tests (NetArchTest) |

## Current State

Only the **Domain** layer is currently implemented:

```
src/CleanArchitecture/
├── CleanArchitecture.Domain/         ✅ Implemented
├── CleanArchitecture.Application/    🔜 Planned
├── CleanArchitecture.Infrastructure/ 🔜 Planned
└── CleanArchitecture.Api/            🔜 Planned
```

### Domain internals

```
CleanArchitecture.Domain/
├── Abstractions/       # Entity base class, IDomainEvent, IRepository, IUnitOfWork
├── Shared/             # Cross-aggregate value objects (Currency, CurrencyType)
├── Users/              # User aggregate
├── Vehicles/           # Vehicle aggregate (with Accessory enum)
└── Rentals/            # Rental aggregate + DomainServices/PriceService
```

### Rental status flow

```
Reserved → Confirmed → Called → Completed
         ↘ Rejected
```

---

## Commands

```bash
# Build
dotnet build

# Code formatting (CSharpier is used in this project)
dotnet csharpier .

# Add a new NuGet package
dotnet add <project-path> package <PackageName>
```
