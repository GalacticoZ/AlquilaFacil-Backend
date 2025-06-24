using NotificationService.Interfaces.Messaging;

namespace NotificationService.Infrastructure.Messaging.Kafka;

public class ConsumerHostedService (IBookingCreatedConsumer consumer) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
    }
}