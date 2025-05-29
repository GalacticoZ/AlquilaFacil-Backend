namespace ProfilesService.Domain.Model.ValueObjects;

public record Phone(string PhoneNumber)
{
    public Phone() : this(String.Empty)
    {
        
    }
}