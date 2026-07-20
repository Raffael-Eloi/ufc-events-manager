using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;

public interface ICreateUfcEvents
{
    Task ExecuteAsync(IEnumerable<UFCEvent> events);
}