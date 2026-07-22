# UFC.Events.Manager.API

## Database migrations

This project uses EF Core with Npgsql (PostgreSQL). Migrations are managed with the `dotnet-ef` CLI tool.

### Prerequisites

Install the EF Core CLI tool globally (one-time setup):

```
dotnet tool install --global dotnet-ef
```

Run migration commands from the API project directory:

```
cd src\UFC.Events.Manager.API
```

### Common commands

Create a new migration after changing the model (`UfcEventContext` / entities):

```
dotnet ef migrations add {YourMigrationName}
```

Remove the last migration (only if it hasn't been applied to a database yet):

```
dotnet ef migrations remove
```

Apply pending migrations to the database:

```
dotnet ef database update
```
