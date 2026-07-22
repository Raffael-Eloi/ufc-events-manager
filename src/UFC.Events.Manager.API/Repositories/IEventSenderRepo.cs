using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

// TODO: Implement concrete repository
public interface IEventSenderRepo
{
    Task CreateCalendarEventAsync(CalendarEvent calendarEvent);
}