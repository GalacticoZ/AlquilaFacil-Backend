using LocalsService.Domain.Model.Commands;

namespace LocalsService.Domain.Services;

public interface ILocalCategoryCommandService
{
    Task Handle(SeedLocalCategoriesCommand command);
}