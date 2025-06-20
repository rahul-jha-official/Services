using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Models;
using System.Text;

namespace RabbitMQ.Service;

public class MessageProducer(IOptions<RabbitMQConfiguration> options) : IMessageProducer, IDisposable
{

    private IConnection? _connection;
    private IChannel? _channel;

    public async Task BasicPublishAsync<T>(ExchangeConfiguration exchangeConfiguration, QueueConfiguration queueConfiguration, MessageConfiguration<T> messageConfiguration) where T : class
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

        var serializedMessage = JsonConvert.SerializeObject(messageConfiguration.Message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        await _channel.BasicPublishAsync(exchange: exchangeConfiguration.Name, 
                                            routingKey: exchangeConfiguration.RouingKey, 
                                            body: body,
                                            mandatory: true,
                                            basicProperties: messageConfiguration.Properties);
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
