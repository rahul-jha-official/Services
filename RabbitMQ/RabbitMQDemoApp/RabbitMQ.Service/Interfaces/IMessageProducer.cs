using RabbitMQ.Service.Models;

namespace RabbitMQ.Service.Interfaces;

public interface IMessageProducer
{
    Task BasicPublishAsync<T>(ExchangeConfiguration exchangeConfiguration, QueueConfiguration queueConfiguration, MessageConfiguration<T> messageConfiguration) where T : class;
}
