using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public interface IUfcEventRepo
{
    Task<IEnumerable<UFCEvent>> CreateAsync(IEnumerable<UFCEvent> events);
    
    Task<IEnumerable<UFCEvent>> GetAsync();
}