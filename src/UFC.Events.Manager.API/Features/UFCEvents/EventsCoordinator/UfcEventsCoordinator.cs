using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

public class UfcEventsCoordinator : IUfcEventsCoordinator
{
    private readonly ICreateUfcEvents _createUfcEvents;

    public UfcEventsCoordinator(ICreateUfcEvents createUfcEvents)
    {
        _createUfcEvents = createUfcEvents;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        await _createUfcEvents.ExecuteAsync(events);
    }
}