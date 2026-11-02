using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsSender;

public sealed class UfcEventsSender : IUfcEventsSender
{
    private readonly IEventSenderRepo _eventSenderRepo;

    public UfcEventsSender(IEventSenderRepo eventSenderRepo)
    {
        _eventSenderRepo = eventSenderRepo;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events, IEnumerable<Subscriber> subscribers)
    {
        List<string> subscriberEmails = subscribers.Select(s => s.Email).ToList();
        
        IEnumerable<Task> sends = events.Select(ufcEvent => _eventSenderRepo.CreateCalendarEventAsync(new CalendarEvent
        {
            Title = ufcEvent.Title(),
            Description = ufcEvent.Description(),
            StartTime = ufcEvent.PreliminaryCardStartTime,
            EndTime = ufcEvent.MainCardStartTime + TimeSpan.FromHours(3),
            SendTo = subscriberEmails
        }));

        await Task.WhenAll(sends);
    }
}