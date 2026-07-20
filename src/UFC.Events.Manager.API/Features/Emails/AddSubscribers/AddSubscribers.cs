using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.Emails.AddSubscribers;

public class AddSubscribers : IAddSubscribers
{
    private readonly ISubscriberRepo _subscriberRepo;

    public AddSubscribers(ISubscriberRepo subscriberRepo)
    {
        _subscriberRepo = subscriberRepo;
    }

    public async Task ExecuteAsync(string email)
    {
        if (await _subscriberRepo.ExistsAsync(email))
        {
            return;
        }

        await _subscriberRepo.AddAsync(new Subscriber { Email = email });
    }
}
