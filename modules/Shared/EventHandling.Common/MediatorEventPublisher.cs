using MediatR;
using RabbitMQ.Client;
using System.Text.Json;

namespace EventHandling.Common
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent eventToPublish) where TEvent : class;
    }
    public class MediatorEventPublisher : IEventPublisher
    {
        private readonly IMediator _mediator;
        public MediatorEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TEvent>(TEvent eventToPublish) where TEvent : class
        {
            // Publish the event in-process (via MediatR)
            await _mediator.Publish(eventToPublish);

           
        }

    }
    public interface IEventHandler<in TEvent> where TEvent : class
    {
        Task Handle(TEvent eventToHandle);
    }
    public class RabbitMQEventPublisher : IEventPublisher
    {
        private readonly IConnection _rabbitMqConnection;

        public RabbitMQEventPublisher(IConnection rabbitMqConnection)
        {
             _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task Publish<TEvent>(TEvent eventToPublish) where TEvent : class
        {
            // Publish the event in-process (via MediatR)

            // Publish the event to RabbitMQ if needed
            using (var channel = _rabbitMqConnection.CreateModel())
            {
                var queueName = typeof(TEvent).Name;
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = JsonSerializer.SerializeToUtf8Bytes(eventToPublish);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        // Publishes events both to MediatR (for in-process) and to RabbitMQ (for microservices)

    }
}
