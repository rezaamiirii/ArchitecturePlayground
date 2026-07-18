# MicroservicesArchitecture

Physical root: `src/MicroservicesArchitecture`. This sample is an architecture skeleton only; it contains no business implementation, domain entities, databases, HTTP clients, event producers, or event consumers.

## SOA to Microservices decomposition

```text
SOA Users Service
    -> Identity Microservice
    -> Customers Microservice

SOA Products Service
    -> Catalog Microservice
    -> Pricing Microservice
    -> Inventory Microservice

SOA Orders Service
    -> Ordering Microservice
    -> Payments Microservice
    -> Notifications Microservice
```

SOA:
Users, Products, and Orders are coarse-grained services.

Microservices:
Each coarse-grained service is decomposed into focused business capabilities.

## Boundaries

- Identity owns authentication, credentials, password management, access tokens, and account activation status. It does not own customer profile or order data.
- Customers owns customer name, email, address, contact information, profile, and preferences. It does not own authentication credentials.
- Catalog owns product identity, name, description, category, attributes, and activation status. It does not own price or stock.
- Pricing owns current price, currency, price history, price rules, and future promotion capability. It does not own descriptions or inventory quantities.
- Inventory owns available stock, reserved stock, stock changes, reservations, and releases. It does not own prices.
- Ordering owns orders, order items, status, customer/product snapshots, unit price snapshot, quantities, line totals, and order total. It does not own credentials, catalog data, current pricing, current inventory, payments, or notifications.
- Payments owns payment requests, status, provider abstraction, successful/failed payments, and refund capability. It may store OrderId, PaymentId, Amount, Currency, and PaymentStatus but does not own the Order aggregate.
- Notifications owns email notifications, future SMS notifications, templates, and delivery status. It is a Worker Service for future asynchronous integration events.

## Architectural rules

Each service is independently executable, deployable, and versioned. Each service will own its own database, and no service can access another service's database. No cross-service `ProjectReference` is allowed. Communication will later use HTTP and integration events, but it is not implemented yet. Domain entities are not shared. Contracts are network contracts owned by one service. BuildingBlocks contains only technical primitives.

## Conceptual future order flow

Documentation only; this flow is not implemented yet.

```text
Client
  -> API Gateway
  -> Ordering Service
      -> Customers Service: validate customer
      -> Catalog Service: obtain product information
      -> Pricing Service: obtain current price
      -> Inventory Service: reserve stock
      -> Payments Service: request payment
      -> Notifications Service: send confirmation
```

## Run commands

```bash
dotnet run --project src/MicroservicesArchitecture/Gateway/Microservices.ApiGateway/Microservices.ApiGateway.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Identity/Microservices.Identity.Api/Microservices.Identity.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Customers/Microservices.Customers.Api/Microservices.Customers.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Catalog/Microservices.Catalog.Api/Microservices.Catalog.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Pricing/Microservices.Pricing.Api/Microservices.Pricing.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Inventory/Microservices.Inventory.Api/Microservices.Inventory.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Ordering/Microservices.Ordering.Api/Microservices.Ordering.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Payments/Microservices.Payments.Api/Microservices.Payments.Api.csproj
dotnet run --project src/MicroservicesArchitecture/Services/Notifications/Microservices.Notifications.Worker/Microservices.Notifications.Worker.csproj
```
