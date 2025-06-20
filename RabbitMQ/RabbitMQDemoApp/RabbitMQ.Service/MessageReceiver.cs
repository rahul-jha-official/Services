using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Models;
using System.Text;

namespace RabbitMQ.Service;

public class MessageReceiver(IOptions<RabbitMQConfiguration> options) : IMessageReceiver, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task ReceiveMessage<T>(ExchangeConfiguration exchangeConfiguration, QueueConfiguration queueConfiguration, Func<T, Task> messageHandler) where T : class
    {
        if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
        {
            await INIT();
        }

        if (_connection is null || _channel is null)
        {
            throw new InvalidOperationException("RabbitMQ connection or channel is not initialized.");
        }

        await _channel.ExchangeDeclareAsync(exchange: exchangeConfiguration.Name, type: exchangeConfiguration.Type);

        await _channel.QueueDeclareAsync(queue: queueConfiguration.QueueName,
                                        durable: queueConfiguration.Durable,
                                        exclusive: queueConfiguration.Exclusive,
                                        autoDelete: queueConfiguration.AutoDelete,
                                        arguments: queueConfiguration.Arguments);

        await _channel.QueueBindAsync(queue: queueConfiguration.QueueName,
                                    exchange: exchangeConfiguration.Name,
                                    routingKey: exchangeConfiguration.RouingKey);

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (message, ea) =>
        {
            var body = ea.Body.ToArray();
            var serializedMessage = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonConvert.DeserializeObject<T>(serializedMessage);
            await messageHandler(deserializedMessage!);
        };

        await _channel.BasicConsumeAsync(queue: queueConfiguration.QueueName, autoAck: true, consumer: consumer);
    }

    private async Task INIT()
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            UserName = options.Value.UserName,
            Password = options.Value.Password,
            Port = options.Value.Port
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
