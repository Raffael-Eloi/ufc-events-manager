using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsSender;

public class UfcEventsSender : IUfcEventsSender
{
    private readonly IEventSenderRepo _eventSenderRepo;

    public UfcEventsSender(IEventSenderRepo eventSenderRepo)
    {
        _eventSenderRepo = eventSenderRepo;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events, IEnumerable<Subscriber> subscribers)
    {
        List<string> subscriberEmails = subscribers.Select(s => s.Email).ToList();
        
        foreach (UFCEvent ufcEvent in events)
        {
            string description = CreateEventDescription(ufcEvent);

            CalendarEvent calendarEvent = new()
            {
                Title = ufcEvent.Name,
                Description = description,
                StartTime = ufcEvent.PreliminaryCardStartTime,
                EndTime = ufcEvent.MainCardStartTime  + TimeSpan.FromHours(3),
                SendTo = subscriberEmails
            };

            await _eventSenderRepo.CreateCalendarEventAsync(calendarEvent);
        }
    }

    private static string CreateEventDescription(UFCEvent ufcEvent)
    {
        return $"{ufcEvent.Arena}, {ufcEvent.City}, {ufcEvent.Country}. Prelims start at {ufcEvent.PreliminaryCardStartTime:t}, Main Card at {ufcEvent.MainCardStartTime:t}.";
    }
}