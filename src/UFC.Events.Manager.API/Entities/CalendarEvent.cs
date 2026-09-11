namespace UFC.Events.Manager.API.Entities;

public sealed class CalendarEvent
{
    public required string Title { get; init; }
    
    public required string Description { get; init; }
    
    public required DateTime StartTime { get; init; }
    
    public required DateTime EndTime { get; init; }
    
    public required List<string> SendTo { get; init; }
}