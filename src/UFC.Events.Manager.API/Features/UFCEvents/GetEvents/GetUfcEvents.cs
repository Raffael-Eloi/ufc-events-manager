using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

public class GetUfcEvents : IGetUfcEvents
{
    private readonly IUfcEventRepo _ufcEventRepo;

    public GetUfcEvents(IUfcEventRepo ufcEventRepo)
    {
        _ufcEventRepo = ufcEventRepo;
    }
    
    public async Task<IEnumerable<UFCEvent>> ExecuteAsync()
    {
        return await _ufcEventRepo.GetAsync();
    }
}