using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Services;

namespace LocalsService.Application.Internal.QueryServices;

public class LocalCategoryQueryService(ILocalCategoryRepository localCategoryRepository) : ILocalCategoryQueryService
{
    public async Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query)
    {
        return await localCategoryRepository.GetAllLocalCategories();
    }
}