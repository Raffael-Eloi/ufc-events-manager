using Moq;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Enums;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Tests.Features.UFCEvents;

internal class UfcEventsSenderTest
{
    private Mock<IEventSenderRepo> _eventSenderRepoMock;
    private IUfcEventsSender _ufcEventsSender;
    private UFCEvent event1;
    private UFCEvent event2;
    private Subscriber subscriber1;
    private Subscriber subscriber2;

    [SetUp]
    public void Setup()
    {
        _eventSenderRepoMock = new Mock<IEventSenderRepo>();

        _ufcEventsSender = new UfcEventsSender(_eventSenderRepoMock.Object);
        
        event1 = new UFCEvent()
        {
            Id = 1,
            Name = "UFC 329 - MCGREGOR x HOLLOWAY",
            Type = UFCEventType.NumberedEvent,
            Number = 329,
            Date = new DateOnly(2026, 07, 11),
            City = "Las Vegas",
            Arena = "T-Mobile Arena",
            Country = "United States",
            PreliminaryCardStartTime = new DateTime(2026, 07, 11, 18, 0, 0, DateTimeKind.Utc),
            MainCardStartTime = new DateTime(2026, 07, 11, 22, 0, 0, DateTimeKind.Utc)
        };
        
        event2 = new  UFCEvent()
        {
            Id = 2,
            Name = "UFC Fight Night - DU PLESSIS x USMAN",
            Type = UFCEventType.FightNight,
            Date = new DateOnly(2026, 07, 18),
            City = "Oklahoma",
            Arena = "Paycom Center",
            Country = "United States",
            PreliminaryCardStartTime = new DateTime(2026, 07, 18, 18, 0, 0, DateTimeKind.Utc),
            MainCardStartTime = new DateTime(2026, 07, 18, 21, 0, 0, DateTimeKind.Utc)
        };

        subscriber1 = new Subscriber
        {
            Id = 1,
            Email = "johndoe@gmail.com"
        };
        
        subscriber2 = new Subscriber
        {
            Id = 2,
            Email = "jane@gmail.com"
        };
    }

    [Test]
    public async Task GivenRequest_WhenThereAreNewEvents_ThenTheEventsShouldBeSent()
    {
        // Arrange
        List<UFCEvent> events = [event1, event2];
        
        List<Subscriber> subscribers = [subscriber1, subscriber2];

        // Act
        await _ufcEventsSender.ExecuteAsync(events, subscribers);

        // Assert
        _eventSenderRepoMock
            .Verify(repo => repo.CreateCalendarEventAsync(It.Is<CalendarEvent>(calendarEvent => 
                    calendarEvent.Title == event1.Name &&
                    calendarEvent.Description == $"{event1.Arena}, {event1.City}, {event1.Country}. Prelims start at {event1.PreliminaryCardStartTime:t}, Main Card at {event1.MainCardStartTime:t}." &&
                    calendarEvent.StartTime == event1.PreliminaryCardStartTime &&
                    calendarEvent.EndTime == event1.MainCardStartTime + TimeSpan.FromHours(3) &&
                    calendarEvent.SendTo.Contains(subscriber1.Email) &&
                    calendarEvent.SendTo.Contains(subscriber2.Email))),
            Times.Once);
        
        _eventSenderRepoMock
            .Verify(repo => repo.CreateCalendarEventAsync(It.Is<CalendarEvent>(calendarEvent => 
                    calendarEvent.Title == event2.Name &&
                    calendarEvent.Description == $"{event2.Arena}, {event2.City}, {event2.Country}. Prelims start at {event2.PreliminaryCardStartTime:t}, Main Card at {event2.MainCardStartTime:t}." &&
                    calendarEvent.StartTime == event2.PreliminaryCardStartTime &&
                    calendarEvent.EndTime == event2.MainCardStartTime + TimeSpan.FromHours(3) &&
                    calendarEvent.SendTo.Contains(subscriber1.Email) &&
                    calendarEvent.SendTo.Contains(subscriber2.Email))),
            Times.Once);
    }
}
