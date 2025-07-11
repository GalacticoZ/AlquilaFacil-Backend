namespace LocalsService.Interfaces.REST.Resources;

public record LocalResource(
    int Id,
    string LocalName,
    string DescriptionMessage,
    string Address,
    int Price,
    int Capacity,
    IEnumerable<string> PhotoUrls,
    string Features,
    int LocalCategoryId,
    int UserId
);