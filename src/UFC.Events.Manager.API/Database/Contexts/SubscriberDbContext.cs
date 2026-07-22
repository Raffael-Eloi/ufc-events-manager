using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Database.Contexts;

// TODO: Add audit log later
public class SubscriberDbContext : DbContext
{
    public DbSet<Subscriber> Subscribers { get; set; }
    
    public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Email).IsRequired().HasMaxLength(500);
            
            entity.HasIndex(x => x.Email).IsUnique();
        });
    }
}