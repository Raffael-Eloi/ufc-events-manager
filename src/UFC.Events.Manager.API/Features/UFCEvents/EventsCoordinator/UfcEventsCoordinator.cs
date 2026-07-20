using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

public class UfcEventsCoordinator : IUfcEventsCoordinator
{
    private readonly ICreateUfcEvents _createUfcEvents;
    
    private readonly IGetUfcEvents _getUfcEvents;

    public UfcEventsCoordinator(ICreateUfcEvents createUfcEvents, IGetUfcEvents getUfcEvents)
    {
        _createUfcEvents = createUfcEvents;
        _getUfcEvents = getUfcEvents;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        IDictionary<string, UFCEvent> existingEventsByName = await GetExistingEvents();
        List<UFCEvent> newEvents = events.Where(ufcEvent => !existingEventsByName.ContainsKey(ufcEvent.Name)).ToList();
        
        await _createUfcEvents.ExecuteAsync(newEvents);
    }

    private async Task<IDictionary<string, UFCEvent>> GetExistingEvents()
    {
        IEnumerable<UFCEvent> existingEvents = await _getUfcEvents.ExecuteAsync();
        return existingEvents.ToDictionary(ufcEvent => ufcEvent.Name);
    }
}