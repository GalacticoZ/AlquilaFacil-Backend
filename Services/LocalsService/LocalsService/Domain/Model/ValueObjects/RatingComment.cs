namespace LocalsService.Domain.Model.ValueObjects;

public record RatingComment(int Value)
{
    public RatingComment() : this(0)
    {
        
    }
}