using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace EventHandling.Common
{
    public class RabbitMQEventConsumer
    {
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQEventConsumer(IConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        public void StartListening<TEvent>() where TEvent : class
        {
            var channel = _connection.CreateModel();
            var queueName = typeof(TEvent).Name;

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<TEvent>(body);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                    await handler.Handle(message);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }

}
