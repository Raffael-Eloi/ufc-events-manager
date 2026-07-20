using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.Emails.GetSubscribers;

public class GetSubscribers : IGetSubscribers
{
    private readonly ISubscriberRepo _subscriberRepo;

    public GetSubscribers(ISubscriberRepo subscriberRepo)
    {
        _subscriberRepo = subscriberRepo;
    }

    public async Task<IEnumerable<Subscriber>> ExecuteAsync()
    {
        return await _subscriberRepo.GetAsync();
    }
}
