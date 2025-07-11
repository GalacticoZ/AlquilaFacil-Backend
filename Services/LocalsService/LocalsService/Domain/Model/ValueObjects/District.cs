namespace LocalsService.Domain.Model.ValueObjects;

public record District(string Value)
{
    public District() : this(string.Empty)
    {
        
    }
}