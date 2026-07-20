using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public interface ISubscriberRepo
{
    Task<IEnumerable<Subscriber>> GetAsync();
}
