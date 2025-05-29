namespace LocalsService.Domain.Model.ValueObjects;

public record DescriptionMessage(string MessageDescription)
{
    public DescriptionMessage() : this(String.Empty)
    {
        
    }
}