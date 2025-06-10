using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using NotificationService.Interfaces;
using Shared.Domain.Model.Events;
using System.Text.Json;
using NotificationService.Domain.Services;

namespace NotificationService.Interfaces.Messaging.Consumers;

public class BookingCreatedConsumer(
    INotificationEventService notificationEventService,
    ILogger<BookingCreatedConsumer> logger) : IBookingCreatedConsumer
{
    public void Consume(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "notification-group",
            BootstrapServers = "kafka:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("booking-created");
        logger.LogInformation("Esperando eventos de reservas...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                var bookingEvent = JsonSerializer.Deserialize<BookingCreatedEvent>(result.Message.Value);
                Console.WriteLine("Mensaje Recibido");
                if (result.Message.Value == null)
                {
                    Console.WriteLine("Mensaje Vacio");
                }
                Console.WriteLine(result.Message.Value);
                if (bookingEvent is not null)
                {
                    var message = $"Reserva confirmada: ID {bookingEvent.BookingId}, Fecha: {bookingEvent.CreatedAt}.";
                    notificationEventService.Handle(bookingEvent).Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while consuming booking event.");
            }
        }
    }

}