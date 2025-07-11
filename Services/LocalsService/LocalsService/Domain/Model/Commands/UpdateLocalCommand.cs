namespace LocalsService.Domain.Model.Commands;

public record UpdateLocalCommand(
    int Id,
    string LocalName,
    string DescriptionMessage,
    string Country,
    string City,
    string District,
    string Street,
    int Price,
    int Capacity,
    string Features,
    int LocalCategoryId,
    int UserId
    );