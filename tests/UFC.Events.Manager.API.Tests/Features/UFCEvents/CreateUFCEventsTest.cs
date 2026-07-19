using Moq;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Enums;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Tests.Features.UFCEvents;

internal class CreateUFCEventsTest
{
    [Test]
    public async Task GivenEvents_WhenCreating_ThenTheEventsShouldBePersisted()
    {
        // Arrange
        UFCEvent event1 = new()
        {
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
        
        UFCEvent event2 = new()
        {
            Name = "UFC Fight Night - DU PLESSIS x USMAN",
            Type = UFCEventType.FightNight,
            Date = new DateOnly(2026, 07, 18),
            City = "Oklahoma",
            Arena = "Paycom Center",
            Country = "United States",
            PreliminaryCardStartTime = new DateTime(2026, 07, 18, 18, 0, 0, DateTimeKind.Utc),
            MainCardStartTime = new DateTime(2026, 07, 18, 21, 0, 0, DateTimeKind.Utc)
        };

        List<UFCEvent> events = [event1,  event2];

        var ufcEventRepoMock = new Mock<IUFCEventRepo>();

        ICreateUFCEvents createUFCEvents = new CreateUFCEvents(ufcEventRepoMock.Object);

        // Act
        await createUFCEvents.ExecuteAsync(events);

        // Assert
        ufcEventRepoMock
            .Verify(repo => repo.CreateAsync(It.Is<IEnumerable<UFCEvent>>(ufcEvents => 
                    ufcEvents.Any(ev => 
                        ev.Name == event1.Name &&
                        ev.Type == event1.Type &&
                        ev.Number == event1.Number &&
                        ev.Date == event1.Date &&                     
                        ev.City == event1.City &&                     
                        ev.Country == event1.Country &&               
                        ev.Arena == event1.Arena &&                   
                        ev.PreliminaryCardStartTime == event1.PreliminaryCardStartTime &&                                            
                        ev.MainCardStartTime == event1.MainCardStartTime
                        ) &&
                    ufcEvents.Any(ev => 
                        ev.Name == event2.Name &&
                        ev.Type == event2.Type &&
                        ev.Number == event2.Number &&
                        ev.Date == event2.Date &&                     
                        ev.City == event2.City &&                     
                        ev.Country == event2.Country &&               
                        ev.Arena == event2.Arena &&                   
                        ev.PreliminaryCardStartTime == event2.PreliminaryCardStartTime &&                                            
                        ev.MainCardStartTime == event2.MainCardStartTime
                    ))), 
            Times.Once);
    }
}