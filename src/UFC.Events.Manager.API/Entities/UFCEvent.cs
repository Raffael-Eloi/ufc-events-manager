using UFC.Events.Manager.API.Enums;

namespace UFC.Events.Manager.API.Entities;

public class UFCEvent
{
    public int? Id { get; set; }
    
    public required string Name { get; set; }
    
    public required UFCEventType Type { get; set; }
    
    public int? Number { get; set; }
    
    public required DateOnly Date { get; set; }
    
    public required string City { get; set; }
    
    public required string Country { get; set; }
    
    public required string Arena { get; set; }
    
    public required DateTime PreliminaryCardStartTime { get; set; }
    
    public required DateTime MainCardStartTime { get; set; }

    public string Title()
    {
        return $"UFC Fight Night - ${Name}";
    }
}