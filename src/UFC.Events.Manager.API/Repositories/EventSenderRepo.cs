using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public class EventSenderRepo : IEventSenderRepo
{
    public Task CreateCalendarEventAsync(CalendarEvent calendarEvent)
    {
        throw new NotImplementedException();
    }
}