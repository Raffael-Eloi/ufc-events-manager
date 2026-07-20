using Moq;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Enums;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

namespace UFC.Events.Manager.API.Tests.Features.UFCEvents;

internal class UfcEventsCoordinatorTest
{
    private Mock<ICreateUfcEvents> _createUfcEventsMock;
    private Mock<IGetUfcEvents> _getUfcEventsMock;
    private IUfcEventsCoordinator _ufcEventsCoordinator;
    private UFCEvent event1;
    private UFCEvent event2;

    [SetUp]
    public void Setup()
    {
        _createUfcEventsMock = new Mock<ICreateUfcEvents>();
        
        _getUfcEventsMock = new Mock<IGetUfcEvents>();

        _ufcEventsCoordinator = new UfcEventsCoordinator(_createUfcEventsMock.Object, _getUfcEventsMock.Object);
        
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
