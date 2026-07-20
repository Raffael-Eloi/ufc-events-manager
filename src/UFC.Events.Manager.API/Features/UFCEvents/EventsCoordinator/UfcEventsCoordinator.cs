using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;

namespace UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

public class UfcEventsCoordinator : IUfcEventsCoordinator
{
    private readonly ICreateUfcEvents _createUfcEvents;
    private readonly IGetUfcEvents _getUfcEvents;
    private readonly IGetSubscribers _getSubscribers;
    private readonly IUfcEventsSender _ufcEventsSender;

    public UfcEventsCoordinator(
        ICreateUfcEvents createUfcEvents, 
        IGetUfcEvents getUfcEvents, 
        IGetSubscribers getSubscribers, 
        IUfcEventsSender ufcEventsSender)
    {
        _createUfcEvents = createUfcEvents;
        _getUfcEvents = getUfcEvents;
        _getSubscribers = getSubscribers;
        _ufcEventsSender = ufcEventsSender;
    }
    
    public async Task ExecuteAsync(IEnumerable<UFCEvent> events)
    {
        IDictionary<string, UFCEvent> existingEventsByName = await GetExistingEvents();
        List<UFCEvent> newEvents = events.Where(ufcEvent => !existingEventsByName.ContainsKey(ufcEvent.Name)).ToList();
        await _createUfcEvents.ExecuteAsync(newEvents);
        IEnumerable<Subscriber> subscribers = await _getSubscribers.ExecuteAsync();
        await _ufcEventsSender.ExecuteAsync(newEvents, subscribers);
    }

    private async Task<IDictionary<string, UFCEvent>> GetExistingEvents()
    {
        IEnumerable<UFCEvent> existingEvents = await _getUfcEvents.ExecuteAsync();
        return existingEvents.ToDictionary(ufcEvent => ufcEvent.Name);
    }
}