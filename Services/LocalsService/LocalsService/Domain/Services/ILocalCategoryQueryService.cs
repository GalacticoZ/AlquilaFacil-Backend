using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.Queries;

namespace LocalsService.Domain.Services;

public interface ILocalCategoryQueryService
{
    Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query);
}
