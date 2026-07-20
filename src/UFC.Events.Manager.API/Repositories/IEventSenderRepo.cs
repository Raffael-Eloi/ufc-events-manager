using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public interface IEventSenderRepo
{
    Task CreateCalendarEventAsync(CalendarEvent calendarEvent);
}