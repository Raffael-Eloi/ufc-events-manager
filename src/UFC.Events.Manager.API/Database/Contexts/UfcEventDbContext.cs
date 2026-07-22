using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Database.Contexts;

// TODO: Add audit log later
public class UfcEventDbContext : DbContext
{
    public DbSet<UFCEvent> Events { get; set; }
    
    public UfcEventDbContext(DbContextOptions<UfcEventDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UFCEvent>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Type).IsRequired();
            entity.Property(x => x.Number);
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.City).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Country).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Arena).IsRequired().HasMaxLength(500);
            entity.Property(x => x.PreliminaryCardStartTime).IsRequired();
            entity.Property(x => x.MainCardStartTime).IsRequired();
            
            entity.HasIndex(x => x.Name).IsUnique();
        });
    }
}