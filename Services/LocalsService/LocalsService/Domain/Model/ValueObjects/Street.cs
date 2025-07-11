namespace LocalsService.Domain.Model.ValueObjects;

public record Street(string Value)
{
    public Street() : this(string.Empty)
    {
        
    }
}