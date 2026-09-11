namespace UFC.Events.Manager.API.Entities;

public sealed class Subscriber
{
    public int? Id { get; init; }

    public required string Email { get; init; }
}
