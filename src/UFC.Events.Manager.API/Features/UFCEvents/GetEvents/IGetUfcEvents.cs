using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

public interface IGetUfcEvents
{
    Task<IEnumerable<UFCEvent>> ExecuteAsync();
}