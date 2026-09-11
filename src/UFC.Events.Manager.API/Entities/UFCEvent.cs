using UFC.Events.Manager.API.Enums;

namespace UFC.Events.Manager.API.Entities;

public class UFCEvent
{
    public int? Id { get; init; }
    
    public required string Name { get; init; }
    
    public required UFCEventType Type { get; init; }
    
    public int? Number { get; init; }
    
    public required DateOnly Date { get; init; }
    
    public required string City { get; init; }
    
    public required string Country { get; init; }
    
    public required string Arena { get; init; }
    
    public required DateTime PreliminaryCardStartTime { get; init; }
    
    public required DateTime MainCardStartTime { get; init; }

    public string Title()
    {
        if (Type == UFCEventType.FightNight)
            return $"UFC Fight Night - {Name}";
        
        return $"UFC {Number} - {Name}";
    }
    
    public string Description() => $"{Arena}, {City}, {Country}. Prelims start at {PreliminaryCardStartTime:t}, Main Card at {MainCardStartTime:t}.";
}