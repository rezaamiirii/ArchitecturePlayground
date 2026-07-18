# Monolith

This sample demonstrates a traditional monolith: one application, one process, one deployment unit, one SQLite database, one shared `AppDbContext`, direct internal dependencies, and a shared transaction boundary.

## Capabilities

- Users
- Products
- Orders

These are internal capabilities of `Monolith.Api`; they are not independently deployable modules or services.

## Internal create-order flow

```text
OrdersController
    -> OrderService
        -> UserRepository
        -> ProductRepository
        -> OrderRepository
        -> AppDbContext
```

`OrderService` directly calls repositories for other capabilities and saves the order and stock changes through the shared DbContext transaction.

## Comparison with Modular Monolith

Monolith: capabilities share one project, one DbContext, and may directly access each other.

Modular Monolith: capabilities are separated into explicit modules with enforced boundaries and contracts.

| Concern | Monolith | Modular Monolith |
| --- | --- | --- |
| Project count | One production project | Multiple module projects |
| Deployment count | One deployment | One deployment |
| Database | One shared database | Often one database with module-owned areas |
| DbContext | One shared DbContext | Separate module DbContexts are common |
| Internal communication | Direct method calls | Module contracts or mediated calls |
| Module boundaries | Not enforced | Explicitly enforced |
| Coupling | Higher and intentional | Lower by design |
| Transaction handling | One local shared transaction | Coordinated across module boundaries |

## Comparison with SOA and Microservices

SOA uses multiple separately running coarse-grained services. Microservices use multiple focused and independently deployable services with independent data ownership. A monolith is one running application containing all capabilities.

## Important clarification

A monolith is not inherently bad. It is often the simplest and most appropriate architecture for small and medium systems. The main risk is uncontrolled coupling as the system grows.

## Commands

```bash
dotnet restore ArchitecturePlayground.slnx
dotnet build ArchitecturePlayground.slnx
dotnet run --project src/SystemArchitectures/Monolith/Monolith.Api/Monolith.Api.csproj
dotnet test src/SystemArchitectures/Monolith/tests/Monolith.Api.Tests/Monolith.Api.Tests.csproj
```
