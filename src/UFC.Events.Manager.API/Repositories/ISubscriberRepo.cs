using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public interface ISubscriberRepo
{
    Task<IEnumerable<Subscriber>> GetAsync();

    Task<bool> ExistsAsync(string email);

    Task AddAsync(Subscriber subscriber);
}
