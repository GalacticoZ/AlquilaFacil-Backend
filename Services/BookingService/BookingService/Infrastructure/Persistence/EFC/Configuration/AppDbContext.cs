using BookingService.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence.EFC.Configuration;

namespace BookingService.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : BaseDbContext(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Reservation>().HasKey(r => r.Id);
        builder.Entity<Reservation>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Reservation>().Property(r => r.StartDate).IsRequired();
        builder.Entity<Reservation>().Property(r => r.EndDate).IsRequired();
        builder.Entity<Reservation>().Property(r => r.VoucherImageUrl).IsRequired();
        builder.Entity<Reservation>().Property(r => r.UserId).IsRequired();
        builder.Entity<Reservation>().Property(r => r.LocalId).IsRequired();
        
        // Apply SnakeCase Naming Convention
        ApplySharedConventions(builder);
    }
}
