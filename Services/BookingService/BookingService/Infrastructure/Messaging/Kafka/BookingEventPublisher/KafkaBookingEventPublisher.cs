using Confluent.Kafka;
using System.Text.Json;
using BookingService.Domain.Messaging;
using Shared.Domain.Model.Events;

namespace BookingService.Infrastructure.Messaging.Kafka.BookingEventPublisher;

public class KafkaBookingEventPublisher : IBookingEventPublisher 
{
    private static IProducer<Null, string> _producer;
    private readonly IConfiguration _configuration;

    public KafkaBookingEventPublisher(IConfiguration configuration) {
        _configuration = configuration;
        var config = new ProducerConfig {
            BootstrapServers = _configuration["Kafka:BootstrapServers"]
       };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
 
    public async Task PublishAsync(BookingCreatedEvent bookingCreatedEvent){
        var json = JsonSerializer.Serialize(bookingCreatedEvent);
        Console.WriteLine(json);
        await _producer.ProduceAsync("booking-created", new Message<Null, string> { Value = json });
        Console.WriteLine("Mensaje Enviado!!!!!!!!!");
    }
}