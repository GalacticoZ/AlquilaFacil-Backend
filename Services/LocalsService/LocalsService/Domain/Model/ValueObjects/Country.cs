namespace LocalsService.Domain.Model.ValueObjects;

public record Country(string Value)
{
    public Country() : this(string.Empty)
    {
        
    }
}