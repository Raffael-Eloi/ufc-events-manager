using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;

public class CreateUfcEvents : ICreateUfcEvents
{
    private readonly IUfcEventRepo _ufcEventRepo;

    public CreateUfcEvents(IUfcEventRepo ufcEventRepo)
    {
        _ufcEventRepo = ufcEventRepo;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        await _ufcEventRepo.CreateAsync(events);
    }
}