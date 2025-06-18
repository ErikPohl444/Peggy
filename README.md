[![Peggy Guggenheim](/docs/people-peggy-guggenheim-1948-calder-mobile-arc-of-petals.jpg)](/docs/people-peggy-guggenheim-1948-calder-mobile-arc-of-petals.jpg)

# Peggy

## Overview
Peggy is a C# console application that manages users, projects, patronages, and patronage payments. It uses Entity Framework Core with SQLite for data storage.

## Prerequisites
- .NET 9.0 SDK
- SQLite

## Setup
1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore` to restore dependencies.
4. Run `dotnet build` to build the project.
5. Run `dotnet run` to start the application.

## Project Structure
- **Models/**: Contains data models for User, Project, Patronage, and PatronagePayment.
- **Data/**: Contains the AppDbContext for database interactions.
- **Services/**: Contains service classes for CRUD operations on models.
- **Tests/**: Contains unit tests for the services.

## Documentation
- [API Users Documentation](docs/api_users.md)
- [System Onboarding](docs/system_onboarding.md)
- **User Acceptance Testing (UAT) Plan**: [docs/uat.md](docs/uat.md)
- **Observability**: [docs/observability.md](docs/observability.md)

## Running Tests
To run the tests, use the following command:
```
dotnet test
```

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request. 