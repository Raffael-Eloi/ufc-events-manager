using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsSender;

public interface IUfcEventsSender
{
    Task ExecuteAsync(IEnumerable<UFCEvent> events, IEnumerable<Subscriber> subscribers);
}