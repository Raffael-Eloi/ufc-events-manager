using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.Emails.GetSubscribers;

public interface IGetSubscribers
{
    Task<IEnumerable<Subscriber>> ExecuteAsync();
}
