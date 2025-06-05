using System.ComponentModel;
using System.Reflection;
using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.ValueObjects;
using LocalsService.Domain.Repositories;
using LocalsService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalsService.Infrastructure.Persistence.EFC.Repositories;

public class LocalCategoryRepository(AppDbContext context)
    : BaseRepository<LocalCategory>(context), ILocalCategoryRepository
{
    public Task<bool> ExistsLocalCategory(EALocalCategoryTypes type)
    {
        var field = type.GetType().GetField(type.ToString());
        var description = ((DescriptionAttribute)field!.GetCustomAttribute(typeof(DescriptionAttribute)))!.Description;
        return Context.Set<LocalCategory>().AnyAsync(x => x.Name == description);
    }

    public async Task<IEnumerable<LocalCategory>> GetAllLocalCategories()
    {
        return await Context.Set<LocalCategory>().ToListAsync();
    }
}
