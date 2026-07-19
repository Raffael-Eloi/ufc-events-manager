using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;

public interface ICreateUFCEvents
{
    Task ExecuteAsync(IEnumerable<UFCEvent> events);
}