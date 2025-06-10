namespace Shared.Domain.Model.Events;

public class BookingCreatedEvent
{
    public int BookingId {get; set;}
    public string UserEmail {get; set;}
    public string Content {get; set;}
    public DateTime CreatedAt {get; set;}
    
    public int OwnerId {get; set;}
}
