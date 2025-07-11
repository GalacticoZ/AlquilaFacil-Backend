using LocalsService.Domain.Model.Aggregates;
using LocalsService.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST.Transform;

public static class LocalResourceFromEntityAssembler
{
    public static LocalResource ToResourceFromEntity(Local local)
    {
        return new LocalResource(
            local.Id, 
            local.LocalName, 
            local.DescriptionMessage,
            local.Address,
            local.Price, 
            local.Capacity,
            local.LocalPhotos.Select(photo => photo.Url),
            local.Features,
            local.LocalCategoryId,
            local.UserId
        );
    }
}