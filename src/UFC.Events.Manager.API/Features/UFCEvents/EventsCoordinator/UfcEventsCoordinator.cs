using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

public class UfcEventsCoordinator : IUfcEventsCoordinator
{
    public Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        throw new NotImplementedException();
    }
}