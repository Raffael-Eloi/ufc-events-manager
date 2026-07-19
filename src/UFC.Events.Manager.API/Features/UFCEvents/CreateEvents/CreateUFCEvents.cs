using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;

public class CreateUFCEvents : ICreateUFCEvents
{
    private readonly IUFCEventRepo _ufcEventRepo;

    public CreateUFCEvents(IUFCEventRepo ufcEventRepo)
    {
        _ufcEventRepo = ufcEventRepo;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        await _ufcEventRepo.CreateAsync(events);
    }
}