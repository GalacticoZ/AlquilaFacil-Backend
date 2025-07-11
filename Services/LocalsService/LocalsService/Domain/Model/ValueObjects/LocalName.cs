namespace LocalsService.Domain.Model.ValueObjects;

public record LocalName(string Value)
{
    public LocalName() : this(String.Empty)
    {
        
    }
}