using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

public interface IUfcEventsCoordinator
{
    Task ExecuteAsync(IEnumerable<UFCEvent> events);
}