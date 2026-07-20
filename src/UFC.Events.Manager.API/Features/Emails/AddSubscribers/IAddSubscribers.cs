namespace UFC.Events.Manager.API.Features.Emails.AddSubscribers;

public interface IAddSubscribers
{
    Task ExecuteAsync(string email);
}
