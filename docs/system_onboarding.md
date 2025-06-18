# System Onboarding

## Overview
This document provides a comprehensive guide to the structure of the code, linked resources for learning how to develop each piece of code, and links to resources for understanding architectural concepts.

## Code Structure
- **Models/**: Contains data models for User, Project, Patronage, and PatronagePayment.
- **Data/**: Contains the AppDbContext for database interactions.
- **Services/**: Contains service classes for CRUD operations on models.
- **Tests/**: Contains unit tests for the services.
- **Interfaces/**: Contains interface definitions for services and repositories.
- **Utils/**: Contains utility/helper classes.

## Development Resources
- **C# and .NET**: [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/)
- **Entity Framework Core**: [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- **SQLite**: [SQLite Documentation](https://www.sqlite.org/docs.html)
- **Unit Testing**: [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)

## Architectural Concepts
- **Dependency Injection**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- **Repository Pattern**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/repository-pattern)
- **Unit of Work Pattern**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/unit-of-work-pattern)
- **API Design**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/web-api)

## Getting Started
1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore` to restore dependencies.
4. Run `dotnet build` to build the project.
5. Run `dotnet run` to start the application.

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request. 