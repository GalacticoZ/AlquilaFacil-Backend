namespace LocalsService.Domain.Model.ValueObjects;

public record TextComment(string Value)
{
    public TextComment() : this(String.Empty)
    {
        
    }
}