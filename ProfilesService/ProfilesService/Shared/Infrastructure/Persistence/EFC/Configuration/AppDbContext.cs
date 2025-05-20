using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace ProfilesService.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        // Enable Audit Fields Interceptors
        builder.AddCreatedUpdatedInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Profile Context
        
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().OwnsOne(p => p.Name,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.Name).HasColumnName("FirstName");
                n.Property(p => p.FatherName).HasColumnName("FatherName");
                n.Property(p => p.MotherName).HasColumnName("MotherName");
            });
        builder.Entity<Profile>().OwnsOne(p => p.PhoneN,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.PhoneNumber).HasColumnName("PhoneNumber");
            });
        builder.Entity<Profile>().OwnsOne(p => p.DocumentN,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.NumberDocument).HasColumnName("NumberDocument");
            });
        builder.Entity<Profile>().OwnsOne(p => p.Birth,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.BirthDate).HasColumnName("BirthDate");
            });
        
        builder.Entity<Profile>().Property(p => p.BankAccountNumber).IsRequired();
        
        builder.Entity<Profile>().Property(p => p.InterbankAccountNumber).IsRequired();
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}