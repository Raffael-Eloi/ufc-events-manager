using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Database.Contexts;
using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public class UfcEventRepo : IUfcEventRepo
{
    private readonly UfcEventDbContext _dbContext;

    public UfcEventRepo(UfcEventDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UFCEvent>> CreateAsync(IEnumerable<UFCEvent> events)
    {
        List<UFCEvent> eventsToCreate = events.ToList();

        _dbContext.Events.AddRange(eventsToCreate);
        await _dbContext.SaveChangesAsync();

        return eventsToCreate;
    }

    public async Task<IEnumerable<UFCEvent>> GetAsync()
    {
        return await _dbContext.Events.AsNoTracking().ToListAsync();
    }
}