# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run tests (when test projects are added)
dotnet test
dotnet test --filter "FullyQualifiedName~ClassName"

# Code formatting (CSharpier is used in this project)
dotnet csharpier .

# Add a new NuGet package
dotnet add <project-path> package <PackageName>
```

**SDK:** .NET 9.0 (pinned in [global.json](global.json) to `9.0.112`)

## Architecture

This is a **Domain-Driven Design (DDD) + Clean Architecture** project for a vehicle rental system. Currently only the Domain layer exists; Application, Infrastructure, and API layers are planned.

### Clean Architecture — Dependency Rule

Dependencies always point **inward**. Outer layers know about inner layers, never the reverse:

```
API → Application → Domain        (never backwards)
Infrastructure → Application → Domain
```

### Full Layer Structure (Professional / Scalable)

| Layer | Project | Responsibility |
|---|---|---|
| **Domain** | `CleanArchitecture.Domain` | Entities, Value Objects, Aggregates, Domain Events, Domain Services, Repository interfaces — zero external dependencies |
| **Application** | `CleanArchitecture.Application` | Use Cases via CQRS (Commands/Queries + Handlers), Domain Event Handlers, DTOs, Validation |
| **Infrastructure** | `CleanArchitecture.Infrastructure` | EF Core, Repository implementations, external HTTP clients, message brokers, caching, auth providers |
| **API** | `CleanArchitecture.Api` | Minimal API / Controllers, middleware, OpenAPI, global exception handler, auth configuration |
| **Workers** *(optional)* | `CleanArchitecture.Workers` | Background jobs, scheduled tasks (Hangfire / Quartz.NET) |
| **Contracts** *(optional)* | `CleanArchitecture.Contracts` | Shared request/response types for consumers or inter-service communication |

### Current State

```
src/CleanArchitecture/
├── CleanArchitecture.Domain/         ✅ Implemented
├── CleanArchitecture.Application/    🔜 Planned
├── CleanArchitecture.Infrastructure/ 🔜 Planned
└── CleanArchitecture.Api/            🔜 Planned
```

### Domain — Internal Structure

```
CleanArchitecture.Domain/
├── Abstractions/             # Entity base class, IDomainEvent, IRepository, IUnitOfWork
├── Shared/                   # Cross-aggregate value objects (Currency, CurrencyType)
├── Users/                    # User aggregate
├── Vehicles/                 # Vehicle aggregate (Accessory enum)
└── Rentals/                  # Rental aggregate + DomainServices/PriceService
```

### Core Patterns

**Entity base class** ([Abstractions/Entity.cs](src/CleanArchitecture/CleanArchitecture.Domain/Abstractions/Entity.cs)): All aggregate roots inherit from `Entity(Guid id)`. It carries a list of `IDomainEvent` instances managed via `RaisedDomainEvent()`, `GetDomainEvents()`, and `ClearDomainEvents()`.

**Aggregates** are `sealed` classes with `private` constructors and `static` factory methods (e.g., `User.Create()`, `Rental.Reserve()`). Factory methods raise domain events.

**Value objects** are C# `record` types (immutable by default). Located either inside their aggregate folder or in `Shared/` if used across aggregates. Examples: `Currency`, `DateRange`, `Email`, `Address`.

**Domain services** live in `DomainServices/` under the relevant aggregate folder. `PriceService` is stateless and calculates rental pricing from a `Vehicle` and `DateRange`:
- Base price = daily rate × days
- Accessories surcharge: 5% for `AppleCart`/`AndroiCart`, 1% for `AirConditioning`/`Maps`
- Adds vehicle `Maintenance` fee

**Repository interfaces** (`IUserRepository`, `IVehicleRepository`) and `IUnitOfWork` are defined in the Domain layer — implementations belong in Infrastructure.

**Domain events** implement `IDomainEvent` (which extends `MediatR.Contracts.INotification`). Handlers will live in the Application layer.

### Rental Status Flow

```
Reserved → Confirmed → Called → Completed
         ↘ Rejected
```

### Key Dependency

`MediatR.Contracts 2.0.1` — used only for the `INotification` marker interface on domain events. Full MediatR will be added to the Application layer.

### Conventions

- Aggregate roots and value objects are `sealed`
- Properties use `private set` (mutable aggregates) or `init` (immutable)
- Comments in the codebase may be in Spanish
- Commit messages follow Conventional Commits with gitmoji (e.g., `feat: :sparkles:`)
