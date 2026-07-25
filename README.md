# Architecture Playground

A practical .NET repository for learning, comparing, and implementing common software architecture styles.

This repository contains multiple implementations of similar business domains using different architectural approaches. The goal is to demonstrate how project structure, dependencies, communication patterns, deployment boundaries, and testing strategies change between architectures.

> This repository is intended for learning and experimentation. The examples focus on architectural concepts rather than production-ready business functionality.

---

## Goals

The main goals of this repository are:

- Demonstrate common software architecture styles with practical .NET examples
- Compare architectural trade-offs using similar domains and modules
- Show dependency and communication patterns between components
- Provide reference implementations for modularity, maintainability, testing, and deployment
- Explore the evolution from a monolith to distributed architectures
- Document common architectural mistakes and anti-patterns

---

## Repository Structure

```text
ArchitecturePlayground/
├── src/
│   ├── ApplicationArchitectures/
│   │   └── CleanArchitecture/
│   │
│   ├── Docs/
│   │
│   └── SystemArchitectures/
│       ├── DistributedModularMonolith/
│       ├── MicroservicesArchitecture/
│       ├── ModularMonolith/
│       ├── Monolith/
│       └── ServiceOrientedArchitecture/
│           ├── BuildingBlocks/
│           ├── Gateway/
│           ├── Services/
│           └── tests/
│
├── .editorconfig
├── .gitattributes
├── .gitignore
├── ArchitecturePlayground.slnx
└── README.md
````

---

## Architecture Categories

The examples are divided into two main categories.

### Application Architectures

Application architectures describe how the internal code of an application is organized.

Examples include:

* Clean Architecture
* Layered Architecture
* Hexagonal Architecture
* Vertical Slice Architecture

Current implementation:

* [Clean Architecture](src/ApplicationArchitectures/CleanArchitecture)

### System Architectures

System architectures describe how applications, modules, services, databases, and infrastructure components are organized and communicate with each other.

Current implementations:

* [Monolith](src/SystemArchitectures/Monolith)
* [Modular Monolith](src/SystemArchitectures/ModularMonolith)
* [Distributed Modular Monolith](src/SystemArchitectures/DistributedModularMonolith)
* [Service-Oriented Architecture](src/SystemArchitectures/ServiceOrientedArchitecture)
* [Microservices Architecture](src/SystemArchitectures/MicroservicesArchitecture)

---

## Implemented Architectures

### Monolith

A single deployable application where all modules and business capabilities are developed and deployed together.

Key characteristics:

* Single deployment unit
* Usually one database
* Direct in-process communication
* Simple development and deployment
* Strong coupling risk as the application grows

Location:

```text
src/SystemArchitectures/Monolith
```

---

### Modular Monolith

A single deployable application divided into independent business modules with explicit boundaries.

Key characteristics:

* Single deployment unit
* In-process module communication
* Clear module ownership
* Controlled dependencies
* Lower operational complexity than distributed systems
* Suitable for many medium and large applications

Location:

```text
src/SystemArchitectures/ModularMonolith
```

---

### Distributed Modular Monolith

A system whose modules are deployed separately but remain strongly coupled through synchronous communication, shared assumptions, or coordinated releases.

This example demonstrates how a system can appear to be composed of independent services while still behaving like a monolith.

Key characteristics:

* Multiple deployable applications
* Synchronous communication between modules
* Runtime dependency between services
* Coordinated deployment requirements
* Risk of cascading failures
* Limited service autonomy

Location:

```text
src/SystemArchitectures/DistributedModularMonolith
```

---

### Service-Oriented Architecture

A distributed architecture organized around reusable business services, often integrated through shared infrastructure and standardized communication contracts.

Key characteristics:

* Business-oriented services
* Shared building blocks
* Centralized integration patterns
* API Gateway or service gateway
* Reusable enterprise capabilities
* Larger service boundaries than microservices

Location:

```text
src/SystemArchitectures/ServiceOrientedArchitecture
```

Internal structure:

```text
ServiceOrientedArchitecture/
├── BuildingBlocks/
├── Gateway/
├── Services/
└── tests/
```

---

### Microservices Architecture

A distributed architecture where independently deployable services own their business logic and data.

Key characteristics:

* Independent deployment
* Database ownership per service
* Explicit API or messaging contracts
* Decentralized decision-making
* Independent scalability
* Increased operational and infrastructure complexity

Location:

```text
src/SystemArchitectures/MicroservicesArchitecture
```

---

### Clean Architecture

An application architecture that organizes code around business rules and dependency inversion.

Key characteristics:

* Domain and application logic remain independent from infrastructure
* Dependencies point inward
* Frameworks are treated as implementation details
* Improved testability
* Clear separation between use cases and external systems

Location:

```text
src/ApplicationArchitectures/CleanArchitecture
```

---

## Example Business Domain

Where practical, architecture examples use similar business modules to make comparison easier.

Typical modules include:

* Products
* Orders
* Users

Using similar modules helps demonstrate how the same business requirements can be implemented differently across architectural styles.

---

## Architecture Comparison

| Architecture                 | Deployment | Communication                      | Data Ownership             | Operational Complexity | Coupling                    |
| ---------------------------- | ---------- | ---------------------------------- | -------------------------- | ---------------------- | --------------------------- |
| Monolith                     | Single     | In-process                         | Shared                     | Low                    | Usually high                |
| Modular Monolith             | Single     | In-process                         | Shared or schema-separated | Low                    | Controlled                  |
| Distributed Modular Monolith | Multiple   | Mostly synchronous                 | Often shared or coupled    | High                   | High                        |
| SOA                          | Multiple   | HTTP, messaging, integration layer | Mixed                      | Medium to high         | Medium                      |
| Microservices                | Multiple   | HTTP, gRPC, messaging              | Service-owned              | High                   | Low when designed correctly |

---

## Technologies

The repository primarily uses:

* .NET
* ASP.NET Core
* Entity Framework Core
* REST APIs
* Dependency Injection
* Automated Tests
* Docker, where applicable
* Messaging and integration patterns, where applicable

Individual architecture examples may use additional libraries and infrastructure.

---

## Prerequisites

Install the following tools before running the examples:

* .NET SDK matching the version defined in the project files
* Visual Studio, JetBrains Rider, or Visual Studio Code
* Docker Desktop for examples that depend on containers
* Git

Check the installed .NET SDK:

```bash
dotnet --info
```

---

## Getting Started

Clone the repository:

```bash
git clone <repository-url>
cd ArchitecturePlayground
```

Restore dependencies:

```bash
dotnet restore ArchitecturePlayground.slnx
```

Build the complete solution:

```bash
dotnet build ArchitecturePlayground.slnx
```

Run all tests:

```bash
dotnet test ArchitecturePlayground.slnx
```

You can also open the solution in Visual Studio:

```text
ArchitecturePlayground.slnx
```

---

## Running an Example

Each architecture has its own directory and may contain its own startup instructions.

Navigate to the desired architecture:

```bash
cd src/SystemArchitectures/ModularMonolith
```

Then inspect its local README or startup project.

For example:

```bash
dotnet run --project <path-to-startup-project>
```

Some distributed examples may require multiple services, databases, or Docker containers to run.

---

## Design Principles

The examples aim to follow these principles:

* Explicit dependencies
* Clear module and service boundaries
* High cohesion
* Low coupling
* Business-oriented organization
* Testable application logic
* Infrastructure isolation
* Observable communication
* Failure-aware distributed design

---

## What This Repository Is Not

This repository is not intended to prescribe one architecture for every project.

There is no universally best architecture.

The correct choice depends on factors such as:

* Business complexity
* Team size
* Deployment requirements
* Scaling requirements
* Release independence
* Operational maturity
* Reliability requirements
* Expected rate of change

In many cases, a well-designed modular monolith is a better starting point than microservices.

---

## Learning Path

A suggested learning order is:

1. Monolith
2. Modular Monolith
3. Clean Architecture
4. Service-Oriented Architecture
5. Distributed Modular Monolith
6. Microservices Architecture

This sequence starts with simpler deployment models and gradually introduces stronger boundaries, distributed communication, independent data ownership, and operational complexity.

---

## Documentation

Additional architectural notes, diagrams, decision records, and comparisons are available in:

```text
src/Docs
```

Recommended documentation topics include:

* Architecture Decision Records
* Module communication
* Service boundaries
* Database ownership
* Distributed transactions
* Event-driven communication
* API Gateway responsibilities
* Observability
* Resilience patterns
* Deployment models

---

## Testing

Depending on the architecture, the repository may include:

* Unit tests
* Integration tests
* Architecture tests
* API tests
* Contract tests
* End-to-end tests

Run the full test suite:

```bash
dotnet test ArchitecturePlayground.slnx
```

---

## Contribution Guidelines

Contributions should:

* Keep each architecture example isolated
* Avoid sharing business code between examples unless it is intentional
* Document architectural decisions
* Include tests for important behaviour
* Avoid unrelated refactoring
* Preserve consistent naming and folder conventions
* Update relevant README files when behaviour or structure changes

Before submitting a pull request:

```bash
dotnet restore ArchitecturePlayground.slnx
dotnet build ArchitecturePlayground.slnx
dotnet test ArchitecturePlayground.slnx
```

---

## Roadmap

Potential future additions:

* Hexagonal Architecture
* Vertical Slice Architecture
* Event-Driven Architecture
* Event Sourcing
* CQRS
* Domain-Driven Design
* Saga patterns
* Transactional Outbox
* API composition
* Contract testing
* Observability with OpenTelemetry
* Kubernetes deployment examples
* Architecture fitness functions

---

## Disclaimer

The implementations are simplified to highlight architectural concepts.

Production systems require additional considerations such as:

* Security
* Authentication and authorization
* Secrets management
* Monitoring and alerting
* Resilience
* Data protection
* Performance
* Deployment automation
* Disaster recovery
* Compliance
