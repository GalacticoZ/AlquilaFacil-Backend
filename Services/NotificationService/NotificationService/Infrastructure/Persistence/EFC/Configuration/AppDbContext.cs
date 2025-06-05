using NotificationService.Domain.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence.EFC.Configuration;

namespace NotificationService.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Place here your entities configuration
        
        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Notification>().Property(n => n.Title).IsRequired();
        builder.Entity<Notification>().Property(n => n.Description).IsRequired();
        builder.Entity<Notification>().Property(n => n.UserId).IsRequired();
        // Apply SnakeCase Naming Convention
        ApplySharedConventions(builder);
    }
}