using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsSender;

public class UfcEventsSender : IUfcEventsSender
{
    public Task ExecuteAsync(IEnumerable<UFCEvent> events, IEnumerable<Subscriber> subscribers)
    {
        throw new NotImplementedException();
    }
}