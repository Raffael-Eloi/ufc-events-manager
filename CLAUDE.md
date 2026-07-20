# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

UFC Events Manager: a .NET 10 ASP.NET Core Web API (minimal hosting model) for managing UFC events. The codebase is very early-stage — entities, feature interactors, and a repository abstraction exist, but there is no repository implementation and no endpoints are mapped in `Program.cs` yet.

## Commands

```
dotnet build                                                        # build the solution (UFC-Events-Manager.slnx)
dotnet test                                                         # run all tests
dotnet test --filter "FullyQualifiedName~GetUfcEventsTest"          # run a single test class
dotnet test --filter "Name=GivenEvents_WhenCreating_ThenTheEventsShouldBePersisted"  # run a single test
dotnet run --project src/UFC.Events.Manager.API                     # run the API
```

Tests use NUnit + Moq, in `tests/UFC.Events.Manager.API.Tests`, referencing the API project directly (no separate contracts/shared project).

## Architecture

- **Feature-folder (vertical slice) structure**: business logic lives under `src/UFC.Events.Manager.API/Features/<Aggregate>/<UseCase>/`, e.g. `Features/UFCEvents/CreateEvents/` and `Features/UFCEvents/GetEvents/`. Each use case is a single-method interactor: an interface (`ICreateUfcEvents`, `IGetUfcEvents`) with one `ExecuteAsync` method, implemented by a class of the same name minus the `I`. Add new use cases by following this same interface+implementation-in-one-folder pattern rather than growing a shared service class.
- **Persistence is abstracted behind `IUfcEventRepo`** (`src/UFC.Events.Manager.API/Repositories/IUfcEventRepo.cs`). Use-case classes depend only on this interface via constructor injection — there is currently no concrete implementation or DI registration wired up, and no database/ORM is configured yet.
- **Entities/Enums are separate from use cases**: `Entities/UFCEvent.cs` and `Enums/UFCEventType.cs`. `UFCEvent` uses C# `required` properties rather than constructors/validation logic.
- `Program.cs` currently only configures OpenAPI (`AddOpenApi`/`MapOpenApi` in Development) and HTTPS redirection; no endpoints, DI registrations, or `app.Run()` have been added yet.

## Development workflow (strict TDD)

Commit history shows this repo is built test-first, with paired RED/GREEN commits (e.g. `... (RED)` then `... (GREEN)`), one behavior per commit. Follow the same discipline for new work: write a failing test, commit it as RED, make it pass with the minimal change, commit as GREEN.

Test naming convention (NUnit, one behavior per test): `GivenX_WhenY_ThenZ`, with `// Arrange` / `// Act` / `// Assert` comments delimiting sections. Dependencies are mocked with Moq (`Mock<IUfcEventRepo>`), and interactors are exercised through their public interface, asserting via `mock.Verify(...)` rather than inspecting internal state.
