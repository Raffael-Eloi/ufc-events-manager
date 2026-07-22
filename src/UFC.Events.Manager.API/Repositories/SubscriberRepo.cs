using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Database.Contexts;
using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

// TODO: Add cancellation token
public class SubscriberRepo : ISubscriberRepo
{
    private readonly SubscriberDbContext _dbContext;

    public SubscriberRepo(SubscriberDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Subscriber>> GetAsync()
    {
        return await _dbContext.Subscribers.AsNoTracking().ToListAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _dbContext.Subscribers.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task AddAsync(Subscriber subscriber)
    {
        _dbContext.Subscribers.Add(subscriber);
        await _dbContext.SaveChangesAsync();
    }
}
