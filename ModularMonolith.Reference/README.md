# Modular Monolith Reference

This reference is an educational modular monolith. It has one deployable ASP.NET Core host and three business modules implemented as class libraries: Users, Products, and Orders.

```text
                 ModularMonolith.Reference.Api
                              |
             -------------------------------------
             |                  |                |
          Users              Products          Orders
             |                  |                |
             -------- Contracts ----------------
```

For order creation the real communication direction is:

```text
Orders.Application
    -> Users.Contracts
    -> Products.Contracts
```

Domain entities remain isolated: Orders stores `UserId` and `ProductId` values plus product name and price snapshots instead of EF navigation properties to Users or Products. That keeps historical orders stable when a product price or name changes and avoids cross-module foreign-key coupling.

## Why modules are class libraries

Each module is a class library so the whole system remains one modular monolith, not microservices. The modules are independently organized and independently persisted, but they are composed into one ASP.NET Core process by the host.

## Controller discovery

Controllers live in module assemblies under each module's `Api.Controllers` namespace. The host explicitly calls `AddUsersModule`, `AddProductsModule`, and `AddOrdersModule`; each registration method adds its assembly as an MVC Application Part.

## Contracts and isolation

Domain entities are not shared. Users exposes `IUsersModule` and `UserOrderInfo`; Products exposes `IProductsModule` and `ProductOrderInfo`. Orders depends only on those contracts, not on `User`, `Product`, `UsersDbContext`, or `ProductsDbContext`.

## Persistence

SQLite is used. Each module has its own EF Core `DbContext`: `UsersDbContext`, `ProductsDbContext`, and `OrdersDbContext`. SQLite does not provide SQL Server-style schemas, so clear table names (`Users`, `Products`, `Orders`, `OrderItems`) are used. Each module owns its own EF configuration and design-time migrations can be added per module.

## Clean Architecture inside modules

Each module is split into `Api`, `Application`, `Contracts`, `Domain`, and `Infrastructure`. Controllers delegate to application services. Application services orchestrate use cases. Domain classes protect invariants through factory methods and behavior. Infrastructure contains EF Core persistence.

## Endpoints

Users:

- `POST /api/users`
- `GET /api/users/{id}`
- `POST /api/users/{id}/activate`
- `POST /api/users/{id}/deactivate`

Products:

- `POST /api/products`
- `GET /api/products/{id}`
- `POST /api/products/{id}/activate`
- `POST /api/products/{id}/deactivate`
- `POST /api/products/{id}/stock`
- `PUT /api/products/{id}/price`

Orders:

- `POST /api/orders`
- `GET /api/orders/{id}`
- `GET /api/orders`

## Run

```bash
dotnet run --project ModularMonolith.Reference/ModularMonolith.Reference.Api/ModularMonolith.Reference.Api.csproj
```

Open Swagger at `/swagger`. Development startup idempotently creates the SQLite database and seeds two active users, one inactive user, three active products, and one inactive product.

## Migrations

Add migrations per module with the module project as the target and the API as the startup project, for example:

```bash
dotnet ef migrations add InitialUsers --project ModularMonolith.Reference/Modules/Users/ModularMonolith.Modules.Users --startup-project ModularMonolith.Reference/ModularMonolith.Reference.Api --context UsersDbContext
```

Repeat for `ProductsDbContext` and `OrdersDbContext`.

## Tests

```bash
dotnet test ArchitecturePlayground.slnx
```

The architecture tests verify module boundaries and controller placement. Integration tests exercise user creation, product creation, order creation, inactive user/product rejection, insufficient stock rejection, stock reduction, and historical price snapshots.
