using LocalsService.Domain.Model.Entities;
using LocalsService.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST.Transform;

public static class LocalCategoryResourceFromEntityAssembler
{
    public static LocalCategoryResource ToResourceFromEntity(LocalCategory localCategory)
    {
        return new LocalCategoryResource(
            localCategory.Id,
            localCategory.Name,
            localCategory.PhotoUrl
            );
    }
}
