using Microsoft.AspNetCore.Mvc;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.Emails.AddSubscribers;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Features.Emails.ValidateEmail;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

namespace UFC.Events.Manager.API.Endpoints;

public sealed class SubscribersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/api/v1.0/subscribers", async (
            [FromServices] IValidateEmail validateEmail,
            [FromServices] IAddSubscribers addSubscribers,
            [FromServices] IGetSubscribers getSubscribers,
            [FromServices] IGetUfcEvents getUfcEvents,
            [FromServices] IUfcEventsSender ufcEventsSender,
            [FromBody] AddSubscriberRequest request) =>
        {
            if (!validateEmail.Execute(request.Email))
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.Email)] = ["The email is not valid."]
                });

            if (await getSubscribers.ExistsAsync(request.Email))
                return Results.Ok();

            await addSubscribers.ExecuteAsync(request.Email);

            IEnumerable<UFCEvent> existingEvents = await getUfcEvents.ExecuteAsync();
            if (existingEvents.Any())
            {
                Subscriber subscriber = new(){ Email = request.Email }    ;
                ufcEventsSender.ExecuteAsync(existingEvents, [subscriber]);
            }

            return Results.Ok();
        });
    }
}

public sealed record AddSubscriberRequest(string Email);