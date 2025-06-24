namespace NotificationService.Interfaces.Messaging;

public interface IBookingCreatedConsumer
{
    public void Consume(CancellationToken stoppingToken);
    
}