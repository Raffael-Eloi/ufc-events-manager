using Microsoft.AspNetCore.Mvc;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

namespace UFC.Events.Manager.API.Endpoints;

// TODO: Add sealed for all the classes that should not be inherited
// TODO: Inject dependencies
// TODO: Add logging to application
// TODO: Provision infrastructure needed
public class UfcEventsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/api/v1.0/ufcevents", async (
            [FromServices] IUfcEventsCoordinator coordinator,
            [FromBody] IEnumerable<UFCEvent> events) =>
        {
            await coordinator.ExecuteAsync(events);
            
            return Results.Ok();
        });
    }
}