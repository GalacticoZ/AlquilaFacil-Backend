namespace LocalsService.Domain.Model.Commands;

public record CreateLocalCommand(
    string LocalName,
    string DescriptionMessage,
    string Country,
    string City,
    string District,
    string Street,
    int Price,
    int Capacity,
    IEnumerable<string> PhotoUrls,
    string Features,
    int LocalCategoryId,
    int UserId
);