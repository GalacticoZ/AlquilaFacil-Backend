namespace ProfilesService.Domain.Model.Commands;

public record CreateProfileCommand(
    string Name, 
    string? FatherName, 
    string? MotherName, 
    string DateOfBirth, 
    string DocumentNumber,
    string Phone, 
    int UserId
    );
