namespace ProfilesService.Domain.Model.ValueObjects;

public record DateOfBirth(string BirthDate)
{
    public DateOfBirth() : this(String.Empty)
    {
        
    }
}