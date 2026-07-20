using Moq;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Enums;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

namespace UFC.Events.Manager.API.Tests.Features.UFCEvents;

internal class UfcEventsCoordinatorTest
{
    private Mock<ICreateUfcEvents> _createUfcEventsMock;
    private Mock<IGetUfcEvents> _getUfcEventsMock;
    private Mock<IGetSubscribers> _getSubscribersMock;
    private Mock<IUfcEventsSender> _ufcEventsSenderMock;
    private IUfcEventsCoordinator _ufcEventsCoordinator;
    private UFCEvent event1;
    private UFCEvent event2;
    private Subscriber subscriber1;
    private Subscriber subscriber2;

    [SetUp]
    public void Setup()
    {
        _createUfcEventsMock = new Mock<ICreateUfcEvents>();
        
        _getUfcEventsMock = new Mock<IGetUfcEvents>();
        
        _getSubscribersMock = new Mock<IGetSubscribers>();
        
        _ufcEventsSenderMock = new Mock<IUfcEventsSender>();

        _ufcEventsCoordinator = new UfcEventsCoordinator(
            _createUfcEventsMock.Object,
            _getUfcEventsMock.Object,
            _getSubscribersMock.Object,
            _ufcEventsSenderMock.Object);
        
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
    public async Task GivenRequest_WhenThereAreNewEvents_ThenTheEventsShouldBePersisted()
    {
        // Arrange
        List<UFCEvent> events = [event1, event2];
        
        // Act
        await _ufcEventsCoordinator.ExecuteAsync(events);

        // Assert
        _createUfcEventsMock
            .Verify(useCase => useCase.ExecuteAsync(events),
            Times.Once);
    }
    
    [Test]
    public async Task GivenRequest_WhenThereAreNewEvents_ThenTheEventsShouldBeSentToCalendar()
    {
        // Arrange
        List<UFCEvent> events = [event1, event2];
        
        List<Subscriber> subscribers = [subscriber1, subscriber2];
        
        _getSubscribersMock
            .Setup(useCase => useCase.ExecuteAsync())
            .ReturnsAsync(subscribers);
        
        // Act
        await _ufcEventsCoordinator.ExecuteAsync(events);

        // Assert
        _ufcEventsSenderMock
            .Verify(useCase => useCase.ExecuteAsync(events, subscribers),
            Times.Once);
    }
    
    [Test]
    public async Task GivenRequest_WhenThereIsNotNewEvents_ThenTheEventsShouldNotBePersisted()
    {
        // Arrange
        List<UFCEvent> events = [event1, event2];
        
        _getUfcEventsMock
            .Setup(useCase => useCase.ExecuteAsync())
            .ReturnsAsync(events);
        
        // Act
        await _ufcEventsCoordinator.ExecuteAsync(events);

        // Assert
        _createUfcEventsMock
            .Verify(useCase => useCase.ExecuteAsync(events),
            Times.Never);
    }
}
