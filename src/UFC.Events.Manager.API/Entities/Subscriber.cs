namespace UFC.Events.Manager.API.Entities;

public class Subscriber
{
    public int? Id { get; init; }

    public required string Email { get; init; }
}
