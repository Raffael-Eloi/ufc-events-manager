using Microsoft.AspNetCore.Mvc;
using UFC.Events.Manager.API.Features.Emails.AddSubscribers;

namespace UFC.Events.Manager.API.Endpoints;

public sealed class SubscribersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/api/v1.0/subscribers", async (
            [FromServices] IAddSubscribers addSubscribers,
            [FromBody] AddSubscriberRequest request) =>
        {
            await addSubscribers.ExecuteAsync(request.Email);

            return Results.Ok();
        });
    }
}

public sealed record AddSubscriberRequest(string Email);
