# Template-WebAPI-DDD-DotNet-Postgres

## About

This template delivers:
1. A Domain-Driven Design a.k.a DDD WebAPI built with C# / .NET
2. The infrastructure to run on the latest PostgreSQL version
3. Documented API including Swagger UI
4. Integration & Unit tests using the XUnit framework
5. Mutation tests using the Stryker framework
6. Continuous Integration a.k.a. CI workflow for GitHub Actions

### Dependencies to execute the application locally

- [.NET 6.0 sdk](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## How to

### Execute the application locally?

In the root directory, execute the command `docker compose up`.

Then, [click here](http://localhost:5001/swagger/index.html) to view the Swagger UI.

Or, execute the command `docker compose up postgres` to execute the Postgres only.

Then, run the application through your IDE and [click here](https://localhost:5001/swagger/index.html) to view the Swagger UI.

### Execute the applications's mutation tests locally?

Execute the command `dotnet tool install -g dotnet-stryker` (one time only).

Then, in the `Tests\Integration` directory, execute the command `dotnet stryker -p Application.csproj`.

Or in the `Tests\Unit` execute the command `dotnet stryker -p Domain.csproj`.

### Generate a migration?

Execute the command `dotnet tool install -g dotnet-ef` (one time only).

Then, in the `Source` directory, execute the command `dotnet ef -p Infrastructure -s Presentation migrations add MIGRATION_NAME`.

If you intend to run the application through Docker, it will auto migrate, but if intend through your IDE:
1. Execute the proper Docker command available above
2. Execute the command `dotnet ef -p Infrastructure -s Presentation database update`
3. Run the application through your IDE