using LocalsService.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Persistence.EFC.Configuration;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.AddInterceptors();
    }

    protected void ApplySharedConventions(ModelBuilder builder)
    {
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}