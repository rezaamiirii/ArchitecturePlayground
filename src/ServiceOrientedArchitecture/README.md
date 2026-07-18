# Service-Oriented Architecture Sample

This sample demonstrates a Service-Oriented Architecture (SOA) skeleton. It is intentionally limited to the physical directory structure, .NET projects, project references, Visual Studio solution folders, minimal startup projects, placeholder folders, and this architecture README.

No business logic, persistence, integration behavior, database access, messaging, controllers, business endpoints, or API Gateway implementation is included yet.

## Architecture overview

- Users, Products, and Orders are independent services.
- Each service has its own API, Application, Domain, Infrastructure, and Contracts projects.
- Service API projects are separate executable processes.
- Services will communicate over HTTP in later tasks.
- The API Gateway is an independent executable and currently has no direct project reference to the services.
- Contracts contain network-facing communication models, not domain entities.
- BuildingBlocks contains only reusable technical primitives shared by architecture samples where appropriate.
- This task creates only the architecture skeleton.

## Physical structure

```text
src/
└── ServiceOrientedArchitecture/
    ├── Gateway/
    │   └── Soa.ApiGateway/
    │       ├── Soa.ApiGateway.csproj
    │       └── Program.cs
    ├── Services/
    │   ├── Users/
    │   │   ├── Soa.Users.Api/
    │   │   │   ├── Soa.Users.Api.csproj
    │   │   │   └── Program.cs
    │   │   ├── Soa.Users.Application/
    │   │   │   └── Soa.Users.Application.csproj
    │   │   ├── Soa.Users.Domain/
    │   │   │   └── Soa.Users.Domain.csproj
    │   │   ├── Soa.Users.Infrastructure/
    │   │   │   └── Soa.Users.Infrastructure.csproj
    │   │   └── Soa.Users.Contracts/
    │   │       └── Soa.Users.Contracts.csproj
    │   ├── Products/
    │   │   ├── Soa.Products.Api/
    │   │   │   ├── Soa.Products.Api.csproj
    │   │   │   └── Program.cs
    │   │   ├── Soa.Products.Application/
    │   │   │   └── Soa.Products.Application.csproj
    │   │   ├── Soa.Products.Domain/
    │   │   │   └── Soa.Products.Domain.csproj
    │   │   ├── Soa.Products.Infrastructure/
    │   │   │   └── Soa.Products.Infrastructure.csproj
    │   │   └── Soa.Products.Contracts/
    │   │       └── Soa.Products.Contracts.csproj
    │   └── Orders/
    │       ├── Soa.Orders.Api/
    │       │   ├── Soa.Orders.Api.csproj
    │       │   └── Program.cs
    │       ├── Soa.Orders.Application/
    │       │   └── Soa.Orders.Application.csproj
    │       ├── Soa.Orders.Domain/
    │       │   └── Soa.Orders.Domain.csproj
    │       ├── Soa.Orders.Infrastructure/
    │       │   └── Soa.Orders.Infrastructure.csproj
    │       └── Soa.Orders.Contracts/
    │           └── Soa.Orders.Contracts.csproj
    ├── BuildingBlocks/
    │   └── Soa.BuildingBlocks/
    │       └── Soa.BuildingBlocks.csproj
    ├── tests/
    │   └── .gitkeep
    └── README.md
```

## Run commands

```bash
dotnet run --project src/ServiceOrientedArchitecture/Gateway/Soa.ApiGateway/Soa.ApiGateway.csproj
```

```bash
dotnet run --project src/ServiceOrientedArchitecture/Services/Users/Soa.Users.Api/Soa.Users.Api.csproj
```

```bash
dotnet run --project src/ServiceOrientedArchitecture/Services/Products/Soa.Products.Api/Soa.Products.Api.csproj
```

```bash
dotnet run --project src/ServiceOrientedArchitecture/Services/Orders/Soa.Orders.Api/Soa.Orders.Api.csproj
```
