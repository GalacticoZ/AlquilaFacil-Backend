using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.ValueObjects;
using LocalsService.Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface ILocalCategoryRepository : IBaseRepository<LocalCategory>
{
    Task<bool> ExistsLocalCategory(EALocalCategoryTypes type);
    Task<IEnumerable<LocalCategory>> GetAllLocalCategories();
}