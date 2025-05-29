using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Interfaces.REST.Resources;

namespace ProfilesService.Interfaces.REST.Transform;

public class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity)
    {
        return new ProfileResource(
            entity.Id, 
            entity.FullName, 
            entity.PhoneNumber, 
            entity.NumberDocument, 
            entity.BirthDate
            );
    }
}