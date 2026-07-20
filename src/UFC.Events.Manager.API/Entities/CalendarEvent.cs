namespace UFC.Events.Manager.API.Entities;

public class CalendarEvent
{
    public required string Title { get; set; }
    
    public required string Description { get; set; }
    
    public required DateTime StartTime { get; set; }
    
    public required DateTime EndTime { get; set; }
    
    public required List<string> SendTo { get; set; }
}