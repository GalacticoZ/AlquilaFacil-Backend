using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence.EFC.Configuration;

namespace LocalsService.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        

        builder.Entity<LocalCategory>().HasKey(c => c.Id);
        builder.Entity<LocalCategory>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LocalCategory>().Property(c => c.Name).IsRequired().HasMaxLength(30);
        builder.Entity<LocalCategory>().Property(c => c.PhotoUrl).IsRequired();


        builder.Entity<LocalCategory>().HasMany<Local>()
            .WithOne()
            .HasForeignKey(t => t.LocalCategoryId);
        
        
        
        
        builder.Entity<Local>().HasKey(p => p.Id);
        builder.Entity<Local>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Local>().Property(p => p.Features).IsRequired();
        builder.Entity<Local>().Property(p => p.Capacity).IsRequired();
        builder.Entity<Local>().OwnsOne(p => p.PricePerHour,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.Value).HasColumnName("PricePerHour");
            });
        builder.Entity<Local>().OwnsOne(p => p.Name,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.Value).HasColumnName("LocalName");
            });
        builder.Entity<Local>().OwnsOne(p => p.Description,
            h =>
            {
                h.WithOwner().HasForeignKey("Id");
                h.Property(g => g.Value).HasColumnName("Description");

            });
        builder.Entity<Local>().OwnsOne(p => p.Country,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(c => c.Value).HasColumnName("Country");

            });
        builder.Entity<Local>().OwnsOne(p => p.City,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(c => c.Value).HasColumnName("City");

            });
        builder.Entity<Local>().OwnsOne(p => p.District,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(d => d.Value).HasColumnName("District");

            });
        builder.Entity<Local>().OwnsOne(p => p.Street,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(d => d.Value).HasColumnName("Street");

            });
        builder.Entity<Local>().HasOne<LocalCategory>().WithMany().HasForeignKey(l => l.LocalCategoryId);
        builder.Entity<Local>().Property(l => l.UserId).IsRequired();
        builder.Entity<Local>()
            .HasMany(l => l.LocalPhotos)
            .WithOne(p => p.Local)
            .HasForeignKey(p => p.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<LocalPhoto>().HasKey(p => p.Id);
        builder.Entity<LocalPhoto>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LocalPhoto>().Property(p => p.Url).IsRequired();

        builder.Entity<Comment>().HasKey(c => c.Id);
        builder.Entity<Comment>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Entity<Comment>().OwnsOne(c => c.Text,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(g => g.Value).HasColumnName("Text");
            });
        
        builder.Entity<Comment>().OwnsOne(c => c.Rating,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(g => g.Value).HasColumnName("Rating");
            });
        

        builder.Entity<Comment>().Property(comment => comment.UserId).IsRequired();
        builder.Entity<Comment>().HasOne<Local>().WithMany().HasForeignKey(l => l.LocalId);
        
        builder.Entity<Report>().HasKey(report => report.Id);
        builder.Entity<Report>().Property(report => report.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Report>().Property(report => report.Description).IsRequired();
        builder.Entity<Report>().Property(report => report.Title).IsRequired();
        builder.Entity<Report>().Property(report => report.CreatedAt).IsRequired();
        builder.Entity<Report>().Property(report => report.UserId).IsRequired();
        builder.Entity<Report>().HasOne<Local>().WithMany().HasForeignKey(r => r.LocalId);
        
        
        // Apply SnakeCase Naming Convention
        ApplySharedConventions(builder);
        
    }
}