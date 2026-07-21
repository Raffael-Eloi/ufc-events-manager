using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Enums;

namespace UFC.Events.Manager.API.Tests.Entities;

public class UfcEventTest
{
    [Test]
    public void GivenName_WhenTypeIsFightNight_ThenTheTitleShouldContainFightNight()
    {
        // Arrange
        UFCEvent fightNight = new()
        {
            Name = "DU PLESSIS x USMAN",
            Type = UFCEventType.FightNight,
            Date = new DateOnly(2026, 07, 18),
            City = "Oklahoma",
            Arena = "Paycom Center",
            Country = "United States",
            PreliminaryCardStartTime = new DateTime(2026, 07, 18, 18, 0, 0, DateTimeKind.Utc),
            MainCardStartTime = new DateTime(2026, 07, 18, 21, 0, 0, DateTimeKind.Utc)
        };
        
        // Act
        string title = fightNight.Title();

        // Assert
        Assert.That(title, Is.EqualTo($"UFC Fight Night - {fightNight.Name}"));
    }
    
    [Test]
    public void GivenName_WhenTypeIsNumberedEvent_ThenTheTitleShouldContainNumberedEvent()
    {
        // Arrange
        UFCEvent numberedEvent = new()
        {
            Name = "MCGREGOR x HOLLOWAY",
            Type = UFCEventType.NumberedEvent,
            Number = 329,
            Date = new DateOnly(2026, 07, 11),
            City = "Las Vegas",
            Arena = "T-Mobile Arena",
            Country = "United States",
            PreliminaryCardStartTime = new DateTime(2026, 07, 11, 18, 0, 0, DateTimeKind.Utc),
            MainCardStartTime = new DateTime(2026, 07, 11, 22, 0, 0, DateTimeKind.Utc)
        };
        
        // Act
        string title = numberedEvent.Title();

        // Assert
        Assert.That(title, Is.EqualTo($"UFC {numberedEvent.Number} - {numberedEvent.Name}"));
    }
}