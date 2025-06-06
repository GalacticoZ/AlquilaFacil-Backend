namespace ProfilesService.Interfaces.REST.Resources;

public record CreateProfileResource(
    string Name,
    string? FatherName,
    string? MotherName,
    string Phone,
    string DocumentNumber, 
    string DateOfBirth,
    int UserId
);