using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public interface IUFCEventRepo
{
    Task<IEnumerable<UFCEvent>> CreateAsync(IEnumerable<UFCEvent> events);
}